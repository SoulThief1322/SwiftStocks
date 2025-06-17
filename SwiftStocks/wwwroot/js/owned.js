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
let symbol = "";
const apiKey = 'cv3ivs9r01ql2eurkv7gcv3ivs9r01ql2eurkv80';
const profileUrl = `https://finnhub.io/api/v1/stock/profile2?symbol=${symbol}&token=${apiKey}`;

let total = 0;
const value = document.querySelector(".value-amount").lastElementChild;

document.addEventListener("DOMContentLoaded", x => {
    const cards = document.querySelectorAll(".owned-stock-card");
    for (let i = 0; i < cards.length; i++) {
        symbol = cards[i].firstElementChild.lastElementChild.getAttribute("data-symbol");
        console.log(symbol);
        fetch(`https://finnhub.io/api/v1/quote?symbol=${symbol}&token=${apiKey}`)
            .then(response => response.json())
            .then(data => {
                if (data && data.c) {
                            cards[i].lastElementChild.textContent = "Current price: " + data.c;
                            total += data.c * cards[i].firstElementChild.firstElementChild.value;
                            value.textContent = Math.round(total * 100) / 100;

                }
            })
            .catch(error => {
                console.error('Error fetching price:', error);
            });
    }
})