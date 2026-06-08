import React from "react";
import logo from "./logo.png";
import "./Navbar.css";
import { Link } from "react-router-dom";
import { useAuth } from "../../Context/useAuth";

interface Props {}

const Navbar = (props: Props) => {
  const { isLoggedIn, user, logout } = useAuth();
  return (
    <nav className="navbar-modern">
      <div className="navbar-modern-container">
        <div className="navbar-modern-left">
          <Link to="/" className="navbar-modern-logo">
            <img src={logo} alt="Vals Logo" className="navbar-modern-img" />
            <span className="navbar-modern-brand">VALS</span>
          </Link>
          <Link to="/search" className="navbar-modern-link">
            Search
          </Link>
        </div>
        <div className="navbar-modern-right">
          {isLoggedIn() ? (
            <>
              <span className="navbar-modern-user">Welcome, {user?.userName}</span>
              <button onClick={logout} className="navbar-modern-btn logout">
                Logout
              </button>
            </>
          ) : (
            <>
              <Link to="/login" className="navbar-modern-link">
                Login
              </Link>
              <Link to="/register" className="navbar-modern-btn signup">
                Signup
              </Link>
            </>
          )}
        </div>
      </div>
    </nav>
  );
};

export default Navbar;