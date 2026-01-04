import { ChangeEvent, SyntheticEvent, useState } from 'react';
import './App.css';
import CardList from './Components/CardList/CardList';
import Search from './Components/Search/Search';
import { CompanySearch } from './company';
import { searchCompanies } from './api';
import ListPortfolio from './Components/Portfolio/ListPortfolio/ListPortfolio';
import Hero from './Components/Hero/Hero';
import Navbar from './Components/Navbar/Navbar';
import { Outlet } from 'react-router-dom';


function App() {
  
  return (
    <>
      <Navbar />
      <Outlet />
    </>
  );
}

export default App;
