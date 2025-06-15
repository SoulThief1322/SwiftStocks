// Search functionality
document.getElementById('searchInput').addEventListener('input', function(e) {
  const searchTerm = e.target.value.toLowerCase();
  const newsCards = document.querySelectorAll('.news-card');

  newsCards.forEach(function(card) {
    const title = card.querySelector('h2').textContent.toLowerCase();
    const content = card.querySelector('p').textContent.toLowerCase();

    if (title.includes(searchTerm) || content.includes(searchTerm)) {
      card.style.display = 'block';
    } else {
      card.style.display = 'none';
    }
  });
});
