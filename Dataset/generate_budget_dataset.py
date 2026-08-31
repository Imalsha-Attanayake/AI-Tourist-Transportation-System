import pandas as pd
import random

# -----------------------------
# Settings
# -----------------------------
n_samples = 500

data = []

# -----------------------------
# Generate dataset
# -----------------------------
for _ in range(n_samples):

    # Inputs
    distance = random.randint(10, 500)
    vehicle = random.choice(["Tuk Tuk", "Car", "SUV", "Van", "Mini Bus"])
    days = random.randint(1, 10)
    fuel_price = random.randint(350, 450)
    passengers = random.randint(1, 15)

    # -----------------------------
    # Cost logic (REALISTIC MODEL)
    # -----------------------------

    if vehicle == "Tuk Tuk":
        cost_per_km = 50
    elif vehicle == "Car":
        cost_per_km = 80
    elif vehicle == "SUV":
        cost_per_km = 120
    elif vehicle == "Van":
        cost_per_km = 150
    else:
        cost_per_km = 200

    base_cost = distance * cost_per_km
    fuel_cost = distance * (fuel_price * 0.1)
    group_factor = passengers * 500
    duration_factor = days * 1000

    estimated_cost = base_cost + fuel_cost + group_factor + duration_factor

    # -----------------------------
    # Store row
    # -----------------------------
    data.append([
        distance,
        vehicle,
        days,
        fuel_price,
        passengers,
        estimated_cost
    ])

# -----------------------------
# Create DataFrame
# -----------------------------
df = pd.DataFrame(data, columns=[
    "Distance_KM",
    "Vehicle_Type",
    "Travel_Days",
    "Fuel_Price",
    "Passengers",
    "Estimated_Cost"
])

# -----------------------------
# Save CSV file
# -----------------------------
df.to_csv("Budget_Estimation_Dataset.csv", index=False)

print("500-row budget dataset created successfully!")