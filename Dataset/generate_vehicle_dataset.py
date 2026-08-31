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

    # Core features
    passengers = random.randint(1, 15)
    duration = random.randint(1, 10)

    luggage = random.choice(["Small", "Medium", "Large"])
    terrain = random.choice(["City", "Beach", "Mountain", "Wildlife", "Mixed"])

    # -----------------------------
    # Realistic budget calculation
    # -----------------------------
    base_budget = passengers * random.randint(8000, 20000)
    budget = base_budget + (duration * random.randint(2000, 5000))

    # -----------------------------
    # Rule-based label (Vehicle Type)
    # -----------------------------
    if passengers <= 2 and budget < 40000:
        vehicle = "Tuk Tuk"
    elif passengers <= 4 and budget < 80000:
        vehicle = "Car"
    elif passengers <= 6:
        vehicle = "SUV"
    elif passengers <= 10:
        vehicle = "Van"
    else:
        vehicle = "Mini Bus"

    # -----------------------------
    # Store row
    # -----------------------------
    data.append([
        passengers,
        budget,
        duration,
        luggage,
        terrain,
        vehicle
    ])

# -----------------------------
# Create DataFrame
# -----------------------------
df = pd.DataFrame(data, columns=[
    "Passengers",
    "Budget_LKR",
    "Travel_Duration_Days",
    "Luggage_Size",
    "Terrain_Type",
    "Vehicle_Type"
])

# -----------------------------
# Show output
# -----------------------------
df = pd.DataFrame(data, columns=[
    "Passengers",
    "Budget_LKR",
    "Travel_Duration_Days",
    "Luggage_Size",
    "Terrain_Type",
    "Vehicle_Type"
])

df.to_csv("Vehicle_Recommendation_Dataset.csv", index=False)

print("500-row dataset created successfully and saved as CSV!")