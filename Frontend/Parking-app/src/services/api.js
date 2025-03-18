import axios from "axios";
import { BASE_URL } from "../utils/constants";

// Configure axios defaults
axios.defaults.baseURL = BASE_URL;

// Create API service
const api = {
  // Cars API
  getCars: async (userId) => {
    const response = await axios.get(`/api/Cars/user/${userId}`);
    return response.data;
  },
  
  addCar: async (userId, licensePlate) => {
    const response = await axios.post(`/api/Cars`, {
      userId,
      licensePlate
    });
    return response.data;
  },
  
  // Parking API
  getActiveSessions: async (userId) => {
    const response = await axios.get(`/api/Parking/active/${userId}`);
    return response.data;
  },
  
  startParking: async (userId, carId) => {
    const response = await axios.post(`/api/Parking/start`, {
      userId,
      carId
    });
    return response.data;
  },
  
  endParking: async (userId, sessionId) => {
    const response = await axios.post(`/api/Parking/end`, {
      userId,
      sessionId
    });
    return response.data;
  },
  
  getParkingHistory: async (userId) => {
    const response = await axios.get(`/api/Parking/history/${userId}`);
    return response.data;
  },
  
  getPricing: async () => {
    const response = await axios.get(`/api/Parking/pricing`);
    return response.data;
  }
};

export default api; 