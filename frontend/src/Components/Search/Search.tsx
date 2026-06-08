import React, { ChangeEvent, JSX, SyntheticEvent } from 'react'
import './Search.css'

interface Props {
    onSearchSubmit: (e: SyntheticEvent) => void;
    search: string | undefined;
    handleSearchChange: (e: ChangeEvent<HTMLInputElement>) => void;
};

const Search: React.FC<Props> = ({ 
    onSearchSubmit,
    search, 
    handleSearchChange,
}): JSX.Element => {  
  return (
    <section className="search-modern-section">
      <div className="search-modern-container">
        <div className="search-modern-header">
          <h1>Find Your Next Investment</h1>
          <p>Search through thousands of stocks and add them to your portfolio</p>
        </div>
        <form className="search-modern-form" onSubmit={onSearchSubmit}>
          <input
            className="search-modern-input"
            id="search-input"
            placeholder="Search companies by name or symbol..."
            value={search}
            onChange={handleSearchChange}
            type="text"
          />
          <button type="submit" className="search-modern-btn">
            Search
          </button>
        </form>
      </div>
    </section>
  )
}

export default Search