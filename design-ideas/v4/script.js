// Mobile Nav
document.getElementById('open-mobile-nav').addEventListener('click', () => {
  document.getElementById('mobile-nav').classList.add('open');
});

document.getElementById('close-mobile-nav').addEventListener('click', () => {
  document.getElementById('mobile-nav').classList.remove('open');
});

// Modal
const modalOverlay = document.getElementById('modal-overlay');
const modalContent = document.getElementById('modal-content');
const closeModalBtn = document.getElementById('close-modal');

document.querySelectorAll('.read-more').forEach(btn => {
  btn.addEventListener('click', () => {
    const title = btn.getAttribute('data-title');
    const content = btn.getAttribute('data-content');
    modalContent.innerHTML = `<h2>${title}</h2><p>${content}</p>`;
    modalOverlay.classList.add('open');
  });
});

closeModalBtn.addEventListener('click', () => {
  modalOverlay.classList.remove('open');
});

modalOverlay.addEventListener('click', (e) => {
  if (e.target === modalOverlay) {
    modalOverlay.classList.remove('open');
  }
});
