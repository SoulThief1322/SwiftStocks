function attemptSellOwned(button) {
    const symbol = button.getAttribute("data-symbol");
    fetch(`/Stocks/CheckOwnership?symbol=${encodeURIComponent(symbol)}`)
        .then(res => {
            if (res.ok) {
                window.location.href = `/Stocks/Sell?symbol=${encodeURIComponent(symbol)}`;
            } else {
                return res.text().then(msg => {
                });
            }
        })
}
