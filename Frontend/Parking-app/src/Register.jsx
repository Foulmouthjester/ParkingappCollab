import { NavBar } from "./NavBar";
import { Footer } from "./Footer";
import { useState } from "react";
import "./Auth.css";

export const Register = () => {
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (formData.password !== formData.confirmPassword) {
      alert("Passwords don't match!");
      return;
    }
    // Add your registration logic here
    console.log("Registration data:", formData);
  };
  return (
    <div>
      <NavBar />
      <div className="auth-container">
        <h2>Sign up page</h2>
        <form onSubmit={handleSubmit}>
          <div className="auth-form-group">
            <input
              type="text"
              name="username"
              placeholder="Username"
              value={formData.username}
              onChange={handleChange}
              required
            />
          </div>

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
              placeholder="Create Password"
              value={formData.password}
              onChange={handleChange}
              required
            />
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
          </div>

          <button type="submit" className="auth-button">
            Register
          </button>

          <p className="auth-switch">
            Already have an account?{" "}
            <a href="/login" className="auth-link">
              Login here
            </a>
          </p>
        </form>
      </div>
      <Footer />
    </div>
  );
};
