import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { BASE_URL } from "../utils/constants";
import "../Auth.css";

export const Login = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });
  
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setApiError("");
    setIsLoading(true);
    
    try {
      // Call the login API endpoint
      const response = await axios.post(`${BASE_URL}/api/Auth/login`, {
        email: formData.email,
        password: formData.password
      });
      
      // Store user info in localStorage for future use
      localStorage.setItem("user", JSON.stringify({
        userId: response.data.userId,
        email: formData.email
      }));
      
      // Redirect to dashboard
      navigate("/dashboard");
    } catch (error) {
      console.error("Login error:", error);
      setApiError(
        error.response?.data || 
        "Login failed. Please check your credentials and try again."
      );
    } finally {
      setIsLoading(false);
    }
  };
  
  return (
    <div>
      <div className="auth-container">
        <h2>Welcome Back</h2>
        {apiError && <p className="auth-error">{apiError}</p>}
        <form onSubmit={handleSubmit}>
          <div className="auth-form-group">
            <input
              type="email"
              name="email"
              placeholder="Email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>

          <div className="auth-form-group">
            <input
              type="password"
              name="password"
              placeholder="Password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>

          <button 
            type="submit" 
            className="auth-button"
            disabled={isLoading}
          >
            {isLoading ? "Logging in..." : "Login"}
          </button>

          <p className="auth-switch">
            Don't have an account?{" "}
            <a href="/register" className="auth-link">
              Register here
            </a>
          </p>
        </form>
      </div>
    </div>
  );
};
