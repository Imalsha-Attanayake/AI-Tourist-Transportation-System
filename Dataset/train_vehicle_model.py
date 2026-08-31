import pandas as pd

# Load the vehicle recommendation dataset
df = pd.read_csv("Vehicle_Recommendation_Dataset.csv")

# Display the first 5 records
print(df.head())

# Check for missing values
print("\nMissing values:")
print(df.isnull().sum())

# Separate input features and target
X = df.drop("Vehicle_Type", axis=1)
y = df["Vehicle_Type"]

print("\nInput features:")
print(X.columns)

print("\nTarget:")
print(y.name)

from sklearn.preprocessing import OneHotEncoder

# Encode categorical input features
categorical_features = ["Luggage_Size", "Terrain_Type"]

encoder = OneHotEncoder(handle_unknown="ignore", sparse_output=False)

X_encoded = encoder.fit_transform(X[categorical_features])

print("\nEncoded categorical features:")
print(X_encoded[:5])

from sklearn.preprocessing import LabelEncoder

# Encode the target variable
label_encoder = LabelEncoder()
y_encoded = label_encoder.fit_transform(y)

print("\nEncoded target:")
print(y_encoded[:10])

print("\nVehicle classes:")
print(label_encoder.classes_)

from sklearn.compose import ColumnTransformer

# Define categorical and numerical features
categorical_features = ["Luggage_Size", "Terrain_Type"]
numerical_features = ["Passengers", "Budget_LKR", "Travel_Duration_Days"]

# Create the preprocessor
preprocessor = ColumnTransformer(
    transformers=[
        ("categorical", OneHotEncoder(handle_unknown="ignore"), categorical_features)
    ],
    remainder="passthrough"
)

# Transform all input features
X_processed = preprocessor.fit_transform(X)

print("\nProcessed feature shape:")
print(X_processed.shape)

from sklearn.model_selection import train_test_split

# Split the dataset into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(
    X_processed,
    y_encoded,
    test_size=0.2,
    random_state=42,
    stratify=y_encoded
)

print("\nTraining data shape:")
print(X_train.shape)

print("\nTesting data shape:")
print(X_test.shape)

from sklearn.ensemble import RandomForestClassifier

# Create the Random Forest model
model = RandomForestClassifier(
    n_estimators=100,
    random_state=42
)

# Train the model
model.fit(X_train, y_train)

print("\nRandom Forest model trained successfully!")

from sklearn.metrics import accuracy_score

# Make predictions on the test data
y_pred = model.predict(X_test)

# Calculate accuracy
accuracy = accuracy_score(y_test, y_pred)

print("\nModel Accuracy:")
print(f"{accuracy * 100:.2f}%")

from sklearn.metrics import classification_report

# Detailed classification report
print("\nClassification Report:")
print(classification_report(
    y_test,
    y_pred,
    target_names=label_encoder.classes_
))

from sklearn.metrics import confusion_matrix

# Create confusion matrix
cm = confusion_matrix(y_test, y_pred)

print("\nConfusion Matrix:")
print(cm)

import joblib

# Save the model and preprocessing components
model_package = {
    "model": model,
    "preprocessor": preprocessor,
    "label_encoder": label_encoder
}

joblib.dump(model_package, "vehicle_recommendation_model.pkl")

print("\nVehicle recommendation model saved successfully!")