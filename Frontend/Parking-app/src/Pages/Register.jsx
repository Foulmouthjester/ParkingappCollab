import { useState } from "react";
import { useNavigate } from "react-router-dom";
import axios from "axios";
import { BASE_URL } from "../utils/constants";
import "../Auth.css";

export const Register = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [errors, setErrors] = useState({});
  const [apiError, setApiError] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const validateForm = () => {
    const newErrors = {};
    
    // Email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      newErrors.email = "Please enter a valid email address";
    }
    
    // Password validation
    if (formData.password.length < 6) {
      newErrors.password = "Password must be at least 6 characters";
    }
    
    // Confirm password
    if (formData.password !== formData.confirmPassword) {
      newErrors.confirmPassword = "Passwords don't match";
    }
    
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setApiError("");
    
    if (validateForm()) {
      setIsLoading(true);
      try {
        await axios.post(`${BASE_URL}/api/Auth/register`, {
          email: formData.email,
          password: formData.password
        });
        
        // Registration successful, redirect to login
        navigate("/login");
      } catch (error) {
        console.error("Registration error:", error);
        if (error.response) {
          setApiError(
            typeof error.response.data === 'string' 
              ? error.response.data 
              : "Registration failed. Please try again."
          );
        } else if (error.request) {
          setApiError("No response from server. Please check your connection.");
        } else {
          setApiError(`Error: ${error.message}`);
        }
      } finally {
        setIsLoading(false);
      }
    }
  };

  return (
    <div>
      <div className="auth-container">
        <h2>Sign up page</h2>
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
            {errors.email && <p className="auth-error">{errors.email}</p>}
          </div>

          <div className="auth-form-group">
            <input
              type="password"
              name="password"
              placeholder="Create Password"
              value={formData.password}
              onChange={handleChange}
              required
            />
            {errors.password && <p className="auth-error">{errors.password}</p>}
          </div>

          <div className="auth-form-group">
            <input
              type="password"
              name="confirmPassword"
              placeholder="Confirm Password"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
            />
            {errors.confirmPassword && <p className="auth-error">{errors.confirmPassword}</p>}
          </div>

          <button 
            type="submit" 
            className="auth-button"
            disabled={isLoading}
          >
            {isLoading ? "Registering..." : "Register"}
          </button>

          <p className="auth-switch">
            Already have an account?{" "}
            <a href="/login" className="auth-link">
              Login here
            </a>
          </p>
        </form>
      </div>
    </div>
  );
};
