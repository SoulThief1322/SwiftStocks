document.getElementById('add-to-watchlist-form').addEventListener('submit', async function(e) {
  e.preventDefault();

  const symbolInput = document.getElementById('stock-symbol');
  const symbol = symbolInput.value.trim().toUpperCase();

  if (!symbol) {
    alert('Please enter a stock symbol.');
    return;
  }

  const apiKey = 'cv3ivs9r01ql2eurkv7gcv3ivs9r01ql2eurkv80';
  const profileUrl = `https://finnhub.io/api/v1/stock/profile2?symbol=${symbol}&token=${apiKey}`;

  try {
    const response = await fetch(profileUrl);
    const data = await response.json();

    if (!data || Object.keys(data).length === 0) {
      alert(`Symbol "${symbol}" does not exist.`);
      return;
    }
    const watchlistName = document.getElementById('watchlist-select').value;

    const postResponse = await fetch('/Stocks/AddSymbolToWatchlist', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
      },
      body: JSON.stringify({ Symbol: symbol, WatchlistName: watchlistName })
    });

    if (!postResponse.ok) {
      const err = await postResponse.text();
      alert(`Error adding symbol: ${err}`);
      return;
    }

    alert(`Symbol "${symbol}" added successfully!`);
    symbolInput.value = '';

  } catch (error) {
    console.error('Error validating symbol:', error);
    alert('Failed to validate symbol. Please try again later.');
  }
});