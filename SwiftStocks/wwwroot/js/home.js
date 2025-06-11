document.addEventListener("DOMContentLoaded", () => {
  fetchTicker();
  setInterval(fetchTicker, 60000);
});

function fetchTicker() {
  const symbols = ["AM", "AAPL", "GOOGL", "MSFT", "AMZN", "TSLA"];
  const apiKey = "cv3ivs9r01ql2eurkv7gcv3ivs9r01ql2eurkv80";
  const ticker = document.getElementById("ticker-content");

  const fetches = symbols.map(symbol => {
    const quoteUrl = `https://finnhub.io/api/v1/quote?symbol=${symbol}&token=${apiKey}`;
    return fetch(quoteUrl)
      .then(res => res.json())
      .then(quote => {
        if (!quote.c || !quote.o) return null;
        const changePercent = ((quote.c - quote.o) / quote.o) * 100;
        const arrow = changePercent >= 0 ? '▲' : '▼';
        return `${symbol} ${quote.c.toFixed(2)} ${arrow}${Math.abs(changePercent).toFixed(1)}%`;
      })
      .catch(() => null);
  });

  Promise.all(fetches).then(results => {
    const tickerText = results.filter(Boolean).join("     ");
    ticker.textContent = tickerText || "Failed to load stock data.";
  });
}