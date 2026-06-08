import React, { SyntheticEvent } from 'react'
import CardPortfolio from '../CardPortfolio/CardPortfolio';
import { PortfolioGet } from '../../../Models/Portfolio';
import './ListPortfolio.css'

interface Props {
    portfolioValues: PortfolioGet[];
    onPortfolioDelete: (symbol: string) => void;
}

const ListPortfolio = ({portfolioValues, onPortfolioDelete}: Props) => {
  return (
    <section id="portfolio-modern">
        <div className="portfolio-modern-header">
          <h2 className="portfolio-modern-title">My Portfolio</h2>
          <p className="portfolio-modern-subtitle">Manage your investment portfolio</p>
        </div>
        <div className="portfolio-modern-grid">
          {portfolioValues && portfolioValues.length > 0 ? (
            portfolioValues.map((portfolioValue) => {
              return (
                <CardPortfolio
                  key={portfolioValue.id}
                  portfolioValue={portfolioValue}
                  onPortfolioDelete={onPortfolioDelete}
                />
              );
            })
          ) : (
            <div className="portfolio-empty">
              <h3>Your portfolio is empty</h3>
              <p>Search for stocks and add them to get started</p>
            </div>
          )}
        </div>
      </section>
  );
}

export default ListPortfolio