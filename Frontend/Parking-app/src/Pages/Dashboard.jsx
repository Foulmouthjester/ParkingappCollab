import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { CarManagement } from "../components/CarManagement";
import { ParkingManagement } from "../components/ParkingManagement";
import { ParkingHistory } from "../components/ParkingHistory";
import "../styles/Dashboard.css";

export const Dashboard = () => {
  const [userId, setUserId] = useState("");
  const [userEmail, setUserEmail] = useState("");
  const [activeTab, setActiveTab] = useState("cars");
  const navigate = useNavigate();
  
  useEffect(() => {
    // Get user info from localStorage
    const user = JSON.parse(localStorage.getItem("user"));
    if (!user) {
      navigate("/login");
      return;
    }
    
    setUserId(user.userId);
    setUserEmail(user.email);
  }, [navigate]);
  
  const handleLogout = () => {
    // Clear user data from localStorage
    localStorage.removeItem("user");
    // Redirect to home page
    navigate("/");
  };
  
  const renderTabContent = () => {
    switch (activeTab) {
      case "cars":
        return <CarManagement userId={userId} />;
      case "parking":
        return <ParkingManagement userId={userId} />;
      case "history":
        return <ParkingHistory userId={userId} />;
      default:
        return <CarManagement userId={userId} />;
    }
  };
  
  return (
    <div className="dashboard-container">
      <div className="dashboard-header">
        <h2>Welcome, {userEmail}!</h2>
        <button onClick={handleLogout} className="logout-button">
          Logout
        </button>
      </div>
      
      <div className="dashboard-tabs">
        <button 
          className={`tab-button ${activeTab === 'cars' ? 'active' : ''}`}
          onClick={() => setActiveTab('cars')}
        >
          My Cars
        </button>
        <button 
          className={`tab-button ${activeTab === 'parking' ? 'active' : ''}`}
          onClick={() => setActiveTab('parking')}
        >
          Parking
        </button>
        <button 
          className={`tab-button ${activeTab === 'history' ? 'active' : ''}`}
          onClick={() => setActiveTab('history')}
        >
          History
        </button>
      </div>
      
      <div className="tab-content">
        {renderTabContent()}
      </div>
    </div>
  );
}; 