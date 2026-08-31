import pandas as pd
import numpy as np
import joblib

from sklearn.compose import ColumnTransformer
from sklearn.preprocessing import OneHotEncoder
from sklearn.model_selection import train_test_split
from sklearn.linear_model import LinearRegression
from sklearn.metrics import mean_absolute_error, mean_squared_error, r2_score


# ============================================================
# LOAD DATASET
# ============================================================

df = pd.read_csv("Budget_Estimation_Dataset.csv")

print("First 5 records:")
print(df.head())


# ============================================================
# CHECK MISSING VALUES
# ============================================================

print("\nMissing values:")
print(df.isnull().sum())


# ============================================================
# VEHICLE COST MAPPING
# Matches the logic used when generating the dataset
# ============================================================

vehicle_cost_per_km = {
    "Tuk Tuk": 50,
    "Car": 80,
    "SUV": 120,
    "Van": 150,
    "Mini Bus": 200
}


# ============================================================
# CREATE ENGINEERED FEATURES
# ============================================================

df["Vehicle_Cost_Per_KM"] = df["Vehicle_Type"].map(
    vehicle_cost_per_km
)

# Vehicle-specific distance cost
df["Distance_Vehicle_Cost"] = (
    df["Distance_KM"] *
    df["Vehicle_Cost_Per_KM"]
)

# Distance × fuel price interaction
df["Distance_Fuel_Cost"] = (
    df["Distance_KM"] *
    df["Fuel_Price"] *
    0.1
)


# ============================================================
# INPUT FEATURES AND TARGET
# ============================================================

X = df[
    [
        "Distance_KM",
        "Vehicle_Type",
        "Travel_Days",
        "Fuel_Price",
        "Passengers",
        "Distance_Vehicle_Cost",
        "Distance_Fuel_Cost"
    ]
]

y = df["Estimated_Cost"]


print("\nInput features:")
print(X.columns)

print("\nTarget:")
print(y.name)


# ============================================================
# CATEGORICAL AND NUMERICAL FEATURES
# ============================================================

categorical_features = [
    "Vehicle_Type"
]

numerical_features = [
    "Distance_KM",
    "Travel_Days",
    "Fuel_Price",
    "Passengers",
    "Distance_Vehicle_Cost",
    "Distance_Fuel_Cost"
]


# ============================================================
# PREPROCESSING
# ============================================================

preprocessor = ColumnTransformer(
    transformers=[
        (
            "categorical",
            OneHotEncoder(
                handle_unknown="ignore"
            ),
            categorical_features
        )
    ],
    remainder="passthrough"
)


# ============================================================
# PROCESS FEATURES
# ============================================================

X_processed = preprocessor.fit_transform(X)

print("\nProcessed feature shape:")
print(X_processed.shape)


# ============================================================
# TRAIN / TEST SPLIT
# ============================================================

X_train, X_test, y_train, y_test = train_test_split(
    X_processed,
    y,
    test_size=0.2,
    random_state=42
)

print("\nTraining data shape:")
print(X_train.shape)

print("\nTesting data shape:")
print(X_test.shape)


# ============================================================
# CREATE LINEAR REGRESSION MODEL
# ============================================================

model = LinearRegression()


# ============================================================
# TRAIN MODEL
# ============================================================

model.fit(
    X_train,
    y_train
)

print("\nLinear Regression model trained successfully!")


# ============================================================
# PREDICTIONS
# ============================================================

y_pred = model.predict(
    X_test
)


# ============================================================
# MODEL EVALUATION
# ============================================================

mae = mean_absolute_error(
    y_test,
    y_pred
)

rmse = np.sqrt(
    mean_squared_error(
        y_test,
        y_pred
    )
)

r2 = r2_score(
    y_test,
    y_pred
)


print("\nModel Evaluation:")

print(
    f"Mean Absolute Error (MAE): {mae:.2f} LKR"
)

print(
    f"Root Mean Squared Error (RMSE): {rmse:.2f} LKR"
)

print(
    f"R² Score: {r2:.4f}"
)


# ============================================================
# SAVE MODEL AND PREPROCESSOR
# ============================================================

model_package = {
    "model": model,
    "preprocessor": preprocessor
}

joblib.dump(
    model_package,
    "budget_estimation_model.pkl"
)

print(
    "\nBudget estimation model saved successfully!"
)