from fastapi import FastAPI
from pydantic import BaseModel
import joblib
import pandas as pd
from pathlib import Path
import heapq


# ============================================================
# FASTAPI APPLICATION
# ============================================================

app = FastAPI(
    title="TravelAI Lanka - AI Service",
    description="AI services for vehicle recommendation, budget estimation and route optimisation.",
    version="1.0"
)


# ============================================================
# MODEL FILE PATHS
# ============================================================

BASE_DIR = Path(__file__).resolve().parent
DATASET_DIR = BASE_DIR.parent / "Dataset"

VEHICLE_MODEL_PATH = DATASET_DIR / "vehicle_recommendation_model.pkl"
BUDGET_MODEL_PATH = DATASET_DIR / "budget_estimation_model.pkl"


# ============================================================
# LOAD TRAINED ML MODELS
# ============================================================

vehicle_package = joblib.load(VEHICLE_MODEL_PATH)
budget_package = joblib.load(BUDGET_MODEL_PATH)

vehicle_model = vehicle_package["model"]
vehicle_preprocessor = vehicle_package["preprocessor"]
vehicle_label_encoder = vehicle_package["label_encoder"]

budget_model = budget_package["model"]
budget_preprocessor = budget_package["preprocessor"]


# ============================================================
# REQUEST MODELS
# ============================================================

class VehicleRecommendationRequest(BaseModel):
    passengers: int
    budget_lkr: float
    travel_duration_days: int
    luggage_size: str
    terrain_type: str


class BudgetEstimationRequest(BaseModel):
    vehicle_type: str
    distance_km: float
    travel_days: int
    fuel_price: float
    passengers: int


class RouteRequest(BaseModel):
    start_location: str
    destination: str


# ============================================================
# SRI LANKAN ROUTE NETWORK
# Distances are in kilometres
# ============================================================

graph = {
    "Colombo": {
        "Kandy": 115,
        "Galle": 125,
        "Negombo": 40
    },

    "Kandy": {
        "Colombo": 115,
        "Nuwara Eliya": 75,
        "Sigiriya": 90
    },

    "Galle": {
        "Colombo": 125,
        "Ella": 230
    },

    "Negombo": {
        "Colombo": 40,
        "Sigiriya": 150
    },

    "Nuwara Eliya": {
        "Kandy": 75,
        "Ella": 55
    },

    "Sigiriya": {
        "Kandy": 90,
        "Negombo": 150,
        "Ella": 200
    },

    "Ella": {
        "Nuwara Eliya": 55,
        "Galle": 230,
        "Sigiriya": 200
    }
}


# ============================================================
# DIJKSTRA ALGORITHM
# ============================================================

def dijkstra(graph, start, destination):

    distances = {
        location: float("inf")
        for location in graph
    }

    previous = {
        location: None
        for location in graph
    }

    distances[start] = 0

    priority_queue = [(0, start)]

    while priority_queue:

        current_distance, current_location = heapq.heappop(
            priority_queue
        )

        if current_distance > distances[current_location]:
            continue

        if current_location == destination:
            break

        for neighbour, distance in graph[current_location].items():

            new_distance = current_distance + distance

            if new_distance < distances[neighbour]:

                distances[neighbour] = new_distance
                previous[neighbour] = current_location

                heapq.heappush(
                    priority_queue,
                    (new_distance, neighbour)
                )

    # Reconstruct route
    route = []

    current = destination

    while current is not None:

        route.append(current)

        current = previous[current]

    route.reverse()

    return route, distances[destination]


# ============================================================
# HOME / HEALTH CHECK ENDPOINT
# ============================================================

@app.get("/")
def home():

    return {
        "message": "TravelAI Lanka AI Service is running!",
        "services": [
            "Vehicle Recommendation",
            "Budget Estimation",
            "Route Optimisation"
        ]
    }


# ============================================================
# VEHICLE RECOMMENDATION
# Random Forest Classification
# ============================================================

@app.post("/api/vehicle-recommendation")
def vehicle_recommendation(
    request: VehicleRecommendationRequest
):

    input_data = pd.DataFrame([
        {
            "Passengers": request.passengers,
            "Budget_LKR": request.budget_lkr,
            "Travel_Duration_Days": request.travel_duration_days,
            "Luggage_Size": request.luggage_size,
            "Terrain_Type": request.terrain_type
        }
    ])

    processed_data = vehicle_preprocessor.transform(
        input_data
    )

    prediction = vehicle_model.predict(
        processed_data
    )

    vehicle_type = vehicle_label_encoder.inverse_transform(
        prediction
    )[0]

    return {
        "recommended_vehicle": vehicle_type
    }


# ============================================================
# BUDGET ESTIMATION
# Linear Regression
# ============================================================

@app.post("/api/budget-estimation")
def budget_estimation(
    request: BudgetEstimationRequest
):

    # Vehicle cost per kilometre
    vehicle_costs = {
        "Tuk Tuk": 50,
        "Car": 80,
        "SUV": 120,
        "Van": 150,
        "Mini Bus": 200
    }

    # Get cost per kilometre for selected vehicle
    cost_per_km = vehicle_costs.get(request.vehicle_type)

    if cost_per_km is None:
        return {
            "error": f"Unknown vehicle type: {request.vehicle_type}",
            "available_vehicle_types": list(vehicle_costs.keys())
        }

    # Create engineered features used during model training
    distance_vehicle_cost = (
        request.distance_km * cost_per_km
    )

    distance_fuel_cost = (
        request.distance_km *
        request.fuel_price *
        0.1
    )

    # Prepare input data
    input_data = pd.DataFrame([
        {
            "Vehicle_Type": request.vehicle_type,
            "Distance_KM": request.distance_km,
            "Travel_Days": request.travel_days,
            "Fuel_Price": request.fuel_price,
            "Passengers": request.passengers,
            "Distance_Vehicle_Cost": distance_vehicle_cost,
            "Distance_Fuel_Cost": distance_fuel_cost
        }
    ])

    # Apply the same preprocessing used during training
    processed_data = budget_preprocessor.transform(
        input_data
    )

    # Predict estimated travel cost
    prediction = budget_model.predict(
        processed_data
    )[0]

    return {
        "estimated_cost_lkr": round(float(prediction), 2)
    }

# ============================================================
# ROUTE OPTIMISATION
# Dijkstra Algorithm
# ============================================================

@app.post("/api/route-optimization")
def route_optimization(
    request: RouteRequest
):

    start = request.start_location
    destination = request.destination

    if start not in graph:

        return {
            "error": f"Unknown start location: {start}",
            "available_locations": list(graph.keys())
        }

    if destination not in graph:

        return {
            "error": f"Unknown destination: {destination}",
            "available_locations": list(graph.keys())
        }

    route, distance = dijkstra(
        graph,
        start,
        destination
    )

    return {
        "route": route,
        "total_distance_km": distance
    }