import React from 'react'
import { Link } from 'react-router-dom';
import { PortfolioGet } from '../../../Models/Portfolio';
import './CardPortfolio.css'

interface Props {
  portfolioValue: PortfolioGet;
  onPortfolioDelete: (symbol: string) => void;
}

const CardPortfolio = ({ portfolioValue, onPortfolioDelete }: Props) => {
  return (
    <div className="card-modern-portfolio">
      <div className="card-modern-header">
        <div className="card-modern-symbol">{portfolioValue.symbol}</div>
        <div className="card-modern-company">{portfolioValue.companyName}</div>
      </div>
      
      <div className="card-modern-price">
        <span className="price-label">Current Price</span>
        <span className="price-value">${portfolioValue.purchase.toFixed(2)}</span>
      </div>

      <Link 
        to={`/company/${portfolioValue.symbol}/company-profile`}
        className="card-modern-link"
      >
        View Details →
      </Link>

      <button 
        onClick={() => onPortfolioDelete(portfolioValue.symbol)}
        className="card-modern-delete"
      >
        Remove from Portfolio
      </button>
    </div>
  );
};

export default CardPortfolio;