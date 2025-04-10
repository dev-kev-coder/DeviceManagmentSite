// Carousel
let index = 0;
const track = document.getElementById('carousel-track');
document.getElementById('next').addEventListener('click', () => {
  index = (index + 1) % 3;
  track.style.transform = `translateX(-${index * 100}%)`;
});
document.getElementById('prev').addEventListener('click', () => {
  index = (index - 1 + 3) % 3;
  track.style.transform = `translateX(-${index * 100}%)`;
});

// Accordion
const accBtn = document.querySelector('.accordion-header');
const accContent = document.querySelector('.accordion-content');
accBtn.addEventListener('click', () => {
  accContent.classList.toggle('open');
});

// Tabs
const tabBtns = document.querySelectorAll('.tab-btn');
tabBtns.forEach(btn => {
  btn.addEventListener('click', () => {
    document.querySelectorAll('.tab-content').forEach(tc => tc.classList.add('hidden'));
    document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
    document.getElementById(`tab-${btn.dataset.tab}`).classList.remove('hidden');
    btn.classList.add('active');
  });
});

// Toast
document.getElementById('show-toast').addEventListener('click', () => {
  const toast = document.getElementById('toast');
  toast.classList.add('show');
  setTimeout(() => toast.classList.remove('show'), 3000);
});

// Modal
const modalOverlay = document.getElementById('modal-overlay');
document.getElementById('open-modal').addEventListener('click', () => {
  modalOverlay.classList.add('open');
});
document.getElementById('close-modal').addEventListener('click', () => {
  modalOverlay.classList.remove('open');
});
