const initHeader = () => {
  const header = document.querySelector('.header');
  const overlay = document.querySelector('.overlay');
  const parentLinks = document.querySelectorAll('.header .parent');
  const nav = document.querySelector('.header__nav');
  const burger = document.querySelector('.header__burger');
  const back = document.querySelectorAll('.submenu__back__link');

  if (window.innerWidth > 1200) {
    parentLinks.forEach((link) => {
      link.addEventListener('mouseenter', () => {
        overlay.classList.add('show');
        header.classList.add('with-overlay');
      });
      link.addEventListener('mouseleave', () => {
        overlay.classList.remove('show');
        header.classList.remove('with-overlay');
      });
    });
  } else {
    parentLinks.forEach((link) => {
      const l = link.querySelector('.header__menu__item__link');

      l.addEventListener('click', (e) => {
        e.preventDefault();

        l.parentElement.querySelector('.submenu').classList.add('show');
        nav.classList.add('show-submenu');
      });
    });
  }

  let oldScrollY = window.scrollY;

  let headerReport = null;
  if (document.querySelector('.report-head')) {
    headerReport = document.querySelector('.report-head');

    const headerReportToggle = headerReport.querySelector(
      '.report-head__toggle',
    );

    headerReportToggle.addEventListener('click', function () {
      headerReport.classList.toggle('show-ctas');
    });

    headerReport.classList.add('no-bg');
  }

  window.addEventListener('scroll', () => {
    const scrollY = window.scrollY;

    if (headerReport) {
      if (scrollY < 15) {
        headerReport.classList.add('no-bg');
      } else {
        headerReport.classList.remove('no-bg');
      }
    }

    if (
      oldScrollY < scrollY &&
      scrollY > 15 &&
      !burger.classList.contains('close')
    ) {
      // down
      header.classList.add('hide');

      if (headerReport) {
        headerReport.classList.add('hide');
      }
    } else {
      // up
      header.classList.remove('hide');

      if (headerReport) {
        headerReport.classList.remove('hide');
      }
    }

    oldScrollY = scrollY;
  });

  // burger
  burger.addEventListener('click', function () {
    burger.classList.toggle('close');
    nav.classList.toggle('show');

    if (!burger.classList.contains('close')) {
      if (document.querySelector('.submenu.show'))
        document.querySelector('.submenu.show').classList.remove('show');
      nav.classList.remove('show-submenu');
      if (window.innerWidth <= 1200) {
        document.documentElement.style.overflow = '';
        document.body.style.overflow = '';
      }
    } else {
      if (window.innerWidth <= 1200) {
        document.documentElement.style.overflow = 'hidden';
        document.body.style.overflow = 'hidden';
      }
    }
  });

  back.forEach((el) => {
    el.addEventListener('click', function () {
      document.querySelector('.submenu.show').classList.remove('show');
      nav.classList.remove('show-submenu');
    });
  });
};

// Call the function after the DOM is loaded
document.addEventListener('DOMContentLoaded', initHeader);
