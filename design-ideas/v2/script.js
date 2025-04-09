document.getElementById('open-nav').addEventListener('click', () => {
  document.getElementById('side-nav').classList.add('open');
});

document.getElementById('close-nav').addEventListener('click', () => {
  document.getElementById('side-nav').classList.remove('open');
});
