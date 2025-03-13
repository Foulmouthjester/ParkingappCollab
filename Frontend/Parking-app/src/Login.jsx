import { NavBar } from "./NavBar";
import { Footer } from "./Footer";
import { useState } from "react";
import "./Auth.css";

export const Login = () => {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    // Add your login logic here
    console.log("Login data:", formData);
  };
  return (
    <div>
      <NavBar />
      <div className="auth-container">
        <h2>Welcome Back</h2>
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

          <button type="submit" className="auth-button">
            Login
          </button>

          <p className="auth-switch">
            Don't have an account?{" "}
            <a href="/register" className="auth-link">
              Register here
            </a>
          </p>
        </form>
      </div>
      <Footer />
    </div>
  );
};
