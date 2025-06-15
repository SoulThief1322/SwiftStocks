document.addEventListener("DOMContentLoaded", () => {
  const filters = {
    "My watchlist": [
      "AAPL",
      "GOOGL",
      "MSFT",
      "AMZN",
      "TSLA",
      "NVDA",
      "META",
      "NFLX",
    ],
    Stocks: [
      "AAPL",
      "MSFT",
      "AMZN",
      "TSLA",
      "META",
      "NVDA",
      "GOOGL",
      "NFLX",
      "INTC",
      "AMD",
    ]
  };

  const navItems = document.querySelectorAll(".top-bar nav ul li");

  navItems.forEach((item) => {
    item.addEventListener("click", () => {
      navItems.forEach((i) => i.classList.remove("active"));
      item.classList.add("active");

      const category = item.textContent.trim();
      const symbols = filters[category] || [];
      fetchTopMovers(symbols);
    });
  });

  fetchTopMovers(filters["My watchlist"]);
});

function fetchTopMovers(symbols) {
  const apiKey = "cv3ivs9r01ql2eurkv7gcv3ivs9r01ql2eurkv80";
  const sidebar = document.querySelector(".sidebar");
  sidebar.innerHTML = "";

  const fetches = symbols.map((symbol) => {
    const quoteUrl = `https://finnhub.io/api/v1/quote?symbol=${symbol}&token=${apiKey}`;
    const profileUrl = `https://finnhub.io/api/v1/stock/profile2?symbol=${symbol}&token=${apiKey}`;

    return Promise.all([fetch(quoteUrl), fetch(profileUrl)])
      .then(([quoteRes, profileRes]) =>
        Promise.all([quoteRes.json(), profileRes.json()])
      )
      .then(([quote, profile]) => {
        if (!quote.c || !quote.o) return null;

        const change = quote.c - quote.o;
        const changePercent = ((change / quote.o) * 100).toFixed(2);
        return {
          name: profile.name || symbol,
          price: `$${quote.c.toFixed(2)}`,
          rawPrice: quote.c.toFixed(2),
          change: `${change >= 0 ? "+" : ""}${change.toFixed(
            2
          )} (${changePercent}%)`,
          changeValue: change,
        };
      })
      .catch((err) => {
        console.error(`Error fetching data for ${symbol}`, err);
        return null;
      });
  });

  Promise.all(fetches).then((results) => {
    const sorted = results
      .filter(Boolean)
      .sort((a, b) => Math.abs(b.changeValue) - Math.abs(a.changeValue))
      .slice(0, 6);

    sorted.forEach((stock, index) => {
      const entry = document.createElement("div");
      entry.classList.add("stock-entry");
      if (index === 0) entry.classList.add("selected");

      const changeClass = stock.changeValue >= 0 ? "up" : "down";

      entry.innerHTML = `
        <div class="stock-title">${stock.name}</div>
        <div class="stock-price">${stock.price}</div>
        <div class="stock-change ${changeClass}">${stock.change}</div>
      `;

      entry.addEventListener("click", () => {
        document
          .querySelectorAll(".stock-entry")
          .forEach((e) => e.classList.remove("selected"));
        entry.classList.add("selected");

        const stockPanel = document.querySelector(".stock-panel");
        stockPanel.innerHTML = `
          <div class="stock-header">
            <span class="stock-name">${stock.name}</span>
            <div class="price-box">
              <div class="sell">SELL <span>${stock.rawPrice}</span></div>
              <div class="buy">BUY <span>${stock.rawPrice}</span></div>
            </div>
            <div class="monthly-change">${stock.change} last month</div>
          </div>
        `;
      });

      sidebar.appendChild(entry);
    });

    const firstEntry = document.querySelector(".stock-entry");
    if (firstEntry) firstEntry.click();
  });
}
