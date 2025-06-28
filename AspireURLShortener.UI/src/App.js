import React, { useState, useEffect } from 'react';
import axios from 'axios';
import './App.css';

function App() {
  const [url, setUrl] = useState('');
  const [shortenedUrls, setShortenedUrls] = useState([]);
  const [page, setPage] = useState(1);
  const [pageSize, setPageSize] = useState(10);
  const [hasNextPage, setHasNextPage] = useState(true);

  const apiBaseUrl = 'http://localhost:5427';

  const fetchUrls = async () => {
    try {
      const response = await axios.get(`${apiBaseUrl}/urls`, {
        params: { page, pageSize },
      });
      const items = response.data.items || [];
      setShortenedUrls(items);
      setHasNextPage(items.length === pageSize);
    } catch (error) {
      console.error('Error fetching URLs:', error);
      setShortenedUrls([]);
      setHasNextPage(false);
    }
  };

  const createShortUrl = async () => {
    if (!url) return;
    try {
      await axios.post(`${apiBaseUrl}/shorten`, { url });
      setUrl('');
      fetchUrls(); // Refresh the list after creating a new URL
    } catch (error) {
      console.error('Error creating short URL:', error);
    }
  };

  useEffect(() => {
    fetchUrls();
  }, [page, pageSize]);

  return (
    <div className="App">
      <h1>URL Shortener</h1>
      <div className="input-container">
        <input
          type="text"
          value={url}
          onChange={(e) => setUrl(e.target.value)}
          placeholder="Enter URL to shorten"
        />
        <button onClick={createShortUrl}>Shorten</button>
      </div>
      <ul className="url-list">
        {shortenedUrls.map((item) => (
          <li key={`${item.shortenedUrl}-${item.originalUrl}`} className="url-item">
            <span>{item.originalUrl}</span>
            <a href={`${item.shortenedUrl}`}>{`${item.shortenedUrl}`}</a>
          </li>
        ))}
      </ul>
      <div className="pagination">
        <button onClick={() => setPage(page - 1)} disabled={page <= 1}>
          Previous
        </button>
        <button onClick={() => setPage(page + 1)} disabled={!hasNextPage}>
          Next
        </button>
      </div>
    </div>
  );
}

export default App;
