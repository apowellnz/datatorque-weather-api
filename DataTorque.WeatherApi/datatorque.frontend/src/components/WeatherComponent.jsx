import React, { useState } from 'react';
import './WeatherComponent.css';

const WeatherComponent = () => {
  const [weatherData, setWeatherData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [latitude, setLatitude] = useState('');
  const [longitude, setLongitude] = useState('');

  const fetchWeatherData = async () => {
    if (!latitude || !longitude) {
      setError('Please enter both latitude and longitude');
      return;
    }

    setLoading(true);
    setError(null);
    setWeatherData(null);

    try {
      const apiBaseUrl = import.meta.env.VITE_API_BASE_URL;
      const response = await fetch(
        `${apiBaseUrl}/weather?latitude=${latitude}&longitude=${longitude}`
      );

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || `HTTP ${response.status}: ${response.statusText}`);
      }

      const data = await response.json();
      setWeatherData(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    fetchWeatherData();
  };

  const useCurrentLocation = () => {
    if (!navigator.geolocation) {
      setError('Geolocation is not supported by this browser');
      return;
    }

    setLoading(true);
    setError(null);

    navigator.geolocation.getCurrentPosition(
      (position) => {
        setLatitude(position.coords.latitude.toString());
        setLongitude(position.coords.longitude.toString());
        setLoading(false);
      },
      (err) => {
        setError(`Location error: ${err.message}`);
        setLoading(false);
      }
    );
  };

  return (
    <div className="weather-component">
      <h2>Weather Information</h2>
      
      <form onSubmit={handleSubmit} className="weather-form">
        <div className="input-group">
          <label htmlFor="latitude">Latitude:</label>
          <input
            type="number"
            id="latitude"
            step="any"
            value={latitude}
            onChange={(e) => setLatitude(e.target.value)}
            placeholder="e.g., -36.8869376"
          />
        </div>
        
        <div className="input-group">
          <label htmlFor="longitude">Longitude:</label>
          <input
            type="number"
            id="longitude"
            step="any"
            value={longitude}
            onChange={(e) => setLongitude(e.target.value)}
            placeholder="e.g., 174.6305024"
          />
        </div>
        
        <div className="button-group">
          <button type="submit" disabled={loading}>
            {loading ? 'Loading...' : 'Get Weather'}
          </button>
          <button type="button" onClick={useCurrentLocation} disabled={loading}>
            Use Current Location
          </button>
        </div>
      </form>

      {error && (
        <div className="error-message">
          <h3>Error:</h3>
          <p>{error}</p>
        </div>
      )}

      {weatherData && (
        <div className="weather-data">
          <h3>Weather Data:</h3>
          <div className="weather-details">
            <div className="weather-item">
              <span className="label">Temperature:</span>
              <span className="value">{weatherData.temperature}°</span>
            </div>
            <div className="weather-item">
              <span className="label">Wind Speed:</span>
              <span className="value">{weatherData.windSpeed} m/s</span>
            </div>
            <div className="weather-item">
              <span className="label">Condition:</span>
              <span className="value">{weatherData.condition}</span>
            </div>
            <div className="weather-item">
              <span className="label">Recommendation:</span>
              <span className="value">{weatherData.recommendation}</span>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default WeatherComponent;
