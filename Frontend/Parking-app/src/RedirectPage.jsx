import { useState, useEffect } from "react";
import { BASE_URL } from "./config";
import "./styles.css"

export default function RedirectPage() {
  const [cars, setCars] = useState([]);
  const [selectedCar, setSelectedCar] = useState(null);
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [newCar, setNewCar] = useState("");
  const [isParking, setIsParking] = useState(false);

  useEffect(() => {
    fetch("/api/cars")
      .then((res) => res.json())
      .then((data) => setCars(data));
  }, []);

  const registerCar = () => {
    fetch("/api/cars", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ name: newCar }),
    }).then(() => {
      setCars([...cars, { name: newCar, cost: 0 }]);
      setNewCar("");
    });
  };

  const selectCar = (car) => {
    setSelectedCar(car);
    setIsConfirmOpen(true);
  };

  const confirmSelection = () => {
    console.log("Car selected:", selectedCar);
    setIsConfirmOpen(false);
  };

  const handleLogout = () => {
    console.log("User logged out");
    localStorage.removeItem("token"); // Remove authentication token
    localStorage.removeItem("user");  // Remove user data
    window.location.href = "/login";  // Redirect to login page
  };

  const removeCar = (carToRemove) => {
    // Assuming the car object has an id for API purposes; adjust if different
    fetch(`/api/cars/${carToRemove.id}`, {
      method: "DELETE",
    }).then(() => {
      // Update local state by filtering out the removed car
      setCars(cars.filter((car) => car.id !== carToRemove.id));
    }).catch((error) => {
      console.error("Error removing car:", error);
    });
  };

  const toggleParking = () => {
    if (isParking) {
      console.log("Parking stopped for:", selectedCar.name);
      // Reset to initial state
      setIsParking(false);
      setSelectedCar(null);
      // Optionally, you could refresh the car list from the server here
      // fetch("/api/cars")
      //   .then((res) => res.json())
      //   .then((data) => setCars(data));
    } else {
      console.log("Parking started for:", selectedCar.name);
      setIsParking(true);
    }
  };

  return (
    <div className="container">
      <div className="header">
        <h1 className="title">Welcome! Register or Select a Car</h1>
        <button className="button logout-button" onClick={handleLogout}>
          Logout
        </button>
      </div>
      <div className="mb-4">
        <input
          type="text"
          className="input-field"
          placeholder="Enter car name"
          value={newCar}
          onChange={(e) => setNewCar(e.target.value)}
        />
        <button 
          className="button" 
          onClick={registerCar}
          disabled={!newCar.trim()} // Button is disabled if newCar is empty or only whitespace
        >
          Register Car
        </button>
      </div>
      <div className="car-list">
        {cars.map((car, index) => (
          <div key={index} onClick={() => selectCar(car)} className="card">
            <p className="text-lg">{car.name}</p>
            <p className="text-sm">Cost: ${car.cost}</p>
            {car.cost === 0 && (
              <button
                className="button remove-button"
                onClick={(e) => {
                  e.stopPropagation(); // Prevents card click from triggering selectCar
                  removeCar(car);
                }}
              >
                Remove Car
              </button>
            )}
          </div>
        ))}
      </div>
      {/* Show selected car and parking controls when a car is confirmed */}
      {selectedCar && !isConfirmOpen && (
        <div className="parking-controls mt-4">
          <p className="text-lg">
            Selected Car: {selectedCar.name} 
            {isParking && " (Parking)"}
          </p>
          <button 
            className={`button ${isParking ? 'bg-red-500' : 'bg-green-500'}`}
            onClick={toggleParking}
          >
            {isParking ? "Stop Parking" : "Start Parking"}
          </button>
        </div>
      )}
      {isConfirmOpen && (
        <div className="dialog">
          <p>Are you sure you want to use {selectedCar?.name}?</p>
          <button className="button" onClick={() => setIsConfirmOpen(false)}>
            Cancel
          </button>
          <button className="button" onClick={confirmSelection}>Confirm</button>
        </div>
      )}
    </div>
  );
}

