import { useState, useEffect } from "react";
import axios from "axios";
import { BASE_URL } from "../utils/constants";
import "../styles/Dashboard.css";

export const ParkingHistory = ({ userId }) => {
  const [history, setHistory] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (userId) {
      fetchParkingHistory();
    }
  }, [userId]);

  const fetchParkingHistory = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get(`${BASE_URL}/api/Parking/history/${userId}`);
      setHistory(response.data);
    } catch (error) {
      console.error("Error fetching parking history:", error);
      setError("Unable to load parking history. Please try again later.");
    } finally {
      setIsLoading(false);
    }
  };

  const formatTime = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleString();
  };

  const calculateDuration = (start, end) => {
    const startDate = new Date(start);
    const endDate = new Date(end);
    const diff = (endDate - startDate) / 1000; // in seconds
    
    const hours = Math.floor(diff / 3600);
    const minutes = Math.floor((diff % 3600) / 60);
    
    return `${hours}h ${minutes}m`;
  };

  return (
    <div className="dashboard-section">
      <h3>Parking History</h3>
      {error && <p className="error-message">{error}</p>}
      
      {isLoading ? (
        <p>Loading history...</p>
      ) : history.length === 0 ? (
        <p>No parking history available.</p>
      ) : (
        <div className="history-table-container">
          <table className="history-table">
            <thead>
              <tr>
                <th>License Plate</th>
                <th>Start Time</th>
                <th>End Time</th>
                <th>Duration</th>
                <th>Cost</th>
              </tr>
            </thead>
            <tbody>
              {history.map(session => (
                <tr key={session.id}>
                  <td>{session.car.licensePlate}</td>
                  <td>{formatTime(session.startTime)}</td>
                  <td>{formatTime(session.endTime)}</td>
                  <td>{calculateDuration(session.startTime, session.endTime)}</td>
                  <td>SEK {session.totalCost.toFixed(2)}</td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}; 