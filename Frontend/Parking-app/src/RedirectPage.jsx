import { useState, useEffect } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import axios from "axios";
import { BASE_URL } from "./config";
import "./styles.css";

const api = axios.create({
  baseURL: BASE_URL || "http://localhost:5110",
  headers: { "Content-Type": "application/json" },
});

export default function RedirectPage() {
  const [cars, setCars] = useState([]);
  const [selectedCar, setSelectedCar] = useState(null);
  const [isConfirmOpen, setIsConfirmOpen] = useState(false);
  const [newCar, setNewCar] = useState("");
  const [isParking, setIsParking] = useState(false);
  const [sessionId, setSessionId] = useState(null);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

  const urlParams = new URLSearchParams(location.search);
  const userId = urlParams.get("userId") || localStorage.getItem("userId") || "1";

  const fetchCars = async () => {
    try {
      const response = await api.get("/api/Parking/cars", { params: { userId } });
      setCars(response.data);
      console.log(`Cars fetched for UserId ${userId}:`, response.data);
    } catch (error) {
      console.error("Error fetching cars:", error.message);
      setError(error.message);
    }
  };

  useEffect(() => {
    fetchCars();
  }, [userId]);

  const registerCar = async () => {
    if (!newCar.trim()) return;
    try {
      const response = await api.post("/api/Parking/cars", { name: newCar }, { params: { userId } });
      setCars([...cars, response.data]);
      console.log("Car registered:", response.data);
      setNewCar("");
    } catch (error) {
      console.error("Error registering car:", error.message);
      setError(error.message);
    }
  };

  const removeCar = async (carId) => {
    console.log("Removing car:", carId);
    setError(null);
    try {
      await api.delete(`/api/Parking/cars/${carId}`, { params: { userId } });
      console.log("Car removed:", carId);
      fetchCars();
    } catch (error) {
      const errorMsg = error.response?.data?.error || error.message;
      console.error("Error removing car:", errorMsg);
      setError(errorMsg);
    }
  };

  const selectCar = (car) => {
    setSelectedCar(car);
    setIsConfirmOpen(true);
  };

  const confirmSelection = () => {
    console.log("Car selected:", selectedCar);
    setIsConfirmOpen(false);
    setIsParking(false);
  };

  const handleLogout = () => {
    console.log("User logged out");
    localStorage.removeItem("userId");
    navigate("/");
  };

  const startParking = async () => {
    if (!selectedCar) return;
    try {
      console.log("Starting parking for:", selectedCar.name);
      const response = await api.post(
        "/api/Parking/start-parking",
        { carId: selectedCar.id },
        { params: { userId } }
      );
      setSessionId(response.data.sessionId);
      setIsParking(true);
      console.log("Parking session started, ID:", response.data.sessionId);
    } catch (error) {
      console.error("Error starting parking:", error.message);
      setError(error.message);
    }
  };

  const stopParking = async () => {
    if (!sessionId) return;
    try {
      console.log("Stopping parking for session:", sessionId);
      const response = await api.post(
        "/api/Parking/end-parking",
        { sessionId },
        { params: { userId } }
      );
      console.log("End parking response:", response.data);
      setIsParking(false);
      setSelectedCar(null);
      setSessionId(null);
      alert(`Parking cost: ${response.data.cost} SEK (to be paid later)`);
      fetchCars();
    } catch (error) {
      console.error("Stop parking error:", error.message);
      setError(error.message);
    }
  };

  const toggleParking = () => {
    if (isParking) {
      stopParking();
    } else {
      startParking();
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
          disabled={!newCar.trim()}
        >
          Register Car
        </button>
      </div>
      {error && <p className="error-text">{error}</p>}
      <div className="car-list">
        {cars.map((car) => (
          <div key={car.id} onClick={() => selectCar(car)} className="card">
            <p className="text-lg">{car.name}</p>
            <p className="text-sm">Cost: {car.cost} SEK</p>
            {car.cost === 0 && (
              <button
                className="button remove-button"
                onClick={(e) => {
                  e.stopPropagation();
                  removeCar(car.id);
                }}
              >
                Remove Car
              </button>
            )}
          </div>
        ))}
      </div>
      {selectedCar && !isConfirmOpen && (
        <div className="parking-controls mt-4">
          <p className="text-lg">
            Selected Car: {selectedCar.name} {isParking && " (Parking)"}
          </p>
          <button
            className={`button ${isParking ? "bg-red-500" : "bg-green-500"}`}
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

