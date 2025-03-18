import { useState, useEffect } from "react";
import axios from "axios";
import { BASE_URL } from "../utils/constants";
import "../styles/Dashboard.css";

export const ParkingManagement = ({ userId }) => {
  const [cars, setCars] = useState([]);
  const [activeSessions, setActiveSessions] = useState([]);
  const [selectedCar, setSelectedCar] = useState("");
  const [isStarting, setIsStarting] = useState(false);
  const [isEnding, setIsEnding] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    if (userId) {
      fetchCars();
      fetchActiveSessions();
    }
  }, [userId]);

  const fetchCars = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/Cars/user/${userId}`);
      setCars(response.data);
      if (response.data.length > 0 && !selectedCar) {
        setSelectedCar(response.data[0].id);
      }
    } catch (error) {
      console.error("Error fetching cars:", error);
    }
  };

  const fetchActiveSessions = async () => {
    try {
      const response = await axios.get(`${BASE_URL}/api/Parking/active/${userId}`);
      setActiveSessions(response.data);
    } catch (error) {
      console.error("Error fetching active sessions:", error);
    }
  };

  const handleStartParking = async () => {
    if (!selectedCar) return;
    
    setIsStarting(true);
    setError("");
    
    try {
      await axios.post(`${BASE_URL}/api/Parking/start`, {
        userId: userId,
        carId: selectedCar
      });
      
      await fetchActiveSessions();
    } catch (error) {
      console.error("Error starting parking:", error);
      setError("Failed to start parking. Please try again.");
    } finally {
      setIsStarting(false);
    }
  };

  const handleEndParking = async (sessionId) => {
    setIsEnding(true);
    setError("");
    
    try {
      await axios.post(`${BASE_URL}/api/Parking/end`, {
        userId: userId,
        sessionId: sessionId
      });
      
      await fetchActiveSessions();
    } catch (error) {
      console.error("Error ending parking:", error);
      setError("Failed to end parking. Please try again.");
    } finally {
      setIsEnding(false);
    }
  };

  const formatTime = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  return (
    <div className="dashboard-section">
      <h3>Parking Management</h3>
      {error && <p className="error-message">{error}</p>}
      
      <div className="start-parking">
        <div className="select-wrapper">
          <select 
            value={selectedCar} 
            onChange={(e) => setSelectedCar(e.target.value)}
            className="car-select"
            disabled={cars.length === 0}
          >
            <option value="">Select a car</option>
            {cars.map(car => (
              <option key={car.id} value={car.id}>
                {car.licensePlate}
              </option>
            ))}
          </select>
        </div>
        
        <button 
          onClick={handleStartParking} 
          disabled={!selectedCar || isStarting || cars.length === 0}
          className="dashboard-button primary-button"
        >
          {isStarting ? "Starting..." : "Start Parking"}
        </button>
      </div>
      
      <div className="active-sessions">
        <h4>Active Parking Sessions</h4>
        
        {activeSessions.length === 0 ? (
          <p>No active parking sessions.</p>
        ) : (
          <ul className="sessions-list">
            {activeSessions.map(session => (
              <li key={session.id} className="session-item">
                <div className="session-info">
                  <span className="car-info">{session.car.licensePlate}</span>
                  <span className="start-time">Started: {formatTime(session.startTime)}</span>
                </div>
                <button
                  onClick={() => handleEndParking(session.id)}
                  disabled={isEnding}
                  className="dashboard-button end-button"
                >
                  {isEnding ? "Ending..." : "End Parking"}
                </button>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}; 