import { useState, useEffect } from "react";
import axios from "axios";
import { BASE_URL } from "../utils/constants";
import "../styles/Dashboard.css";

export const CarManagement = ({ userId, onCarAdded }) => {
  const [cars, setCars] = useState([]);
  const [newCar, setNewCar] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (userId) {
      fetchCars();
    }
  }, [userId]);

  const fetchCars = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/Cars/user/${userId}`);
      setCars(response.data);
    } catch (error) {
      console.error("Error fetching cars:", error);
      setError("Unable to load your cars. Please try again later.");
    }
  };

  const handleAddCar = async (e) => {
    e.preventDefault();
    if (!newCar.trim()) return;
    
    setIsLoading(true);
    setError("");
    
    try {
      await axios.post(`${BASE_URL}/api/Cars`, {
        userId: userId,
        licensePlate: newCar.trim()
      });
      
      setNewCar("");
      await fetchCars();
      if (onCarAdded) onCarAdded();
    } catch (error) {
      console.error("Error adding car:", error);
      setError("Failed to add car. Please try again.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="dashboard-section">
      <h3>Your Cars</h3>
      {error && <p className="error-message">{error}</p>}
      
      <form onSubmit={handleAddCar} className="add-car-form">
        <input
          type="text"
          value={newCar}
          onChange={(e) => setNewCar(e.target.value)}
          placeholder="Enter license plate"
          className="dashboard-input"
        />
        <button 
          type="submit" 
          className="dashboard-button"
          disabled={isLoading}
        >
          {isLoading ? "Adding..." : "Add Car"}
        </button>
      </form>
      
      <div className="cars-list">
        {cars.length === 0 ? (
          <p>No cars registered yet. Add your first car above.</p>
        ) : (
          <ul>
            {cars.map(car => (
              <li key={car.id} className="car-item">
                <span className="license-plate">{car.licensePlate}</span>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}; 