const openMobileNav = document.getElementById('open-mobile-nav');
const closeMobileNav = document.getElementById('close-mobile-nav');
const mobileNav = document.getElementById('mobile-nav');

openMobileNav.addEventListener('click', () => {
  mobileNav.classList.add('open');
});

closeMobileNav.addEventListener('click', () => {
  mobileNav.classList.remove('open');
});
