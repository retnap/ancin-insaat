// Ançın İnşaat — Site Scripts
// Milestone 2 · Commit 2 — Navbar dropdown menus (desktop only, ≥1024px).
// Milestone 2 · Commit 3 — Mobile menu (hamburger-triggered panel).
// Implements the WAI-ARIA Menu Button pattern for the Corporate and
// People First dropdowns on desktop, and a tap-to-expand accordion
// version of the same two dropdowns inside the mobile panel.

(function () {
  'use strict';

  var desktopQuery = window.matchMedia('(min-width: 1024px)');

  function isDesktop() {
    return desktopQuery.matches;
  }

  // --------------------------------------------------------------------
  // Mobile Menu Panel
  // --------------------------------------------------------------------

  var navToggle = document.getElementById('navbar-toggle');
  var navPanel = document.getElementById('navbar-nav');
  var panelOpen = false;

  function openPanel() {
    if (panelOpen || !navToggle || !navPanel) {
      return;
    }

    panelOpen = true;
    navToggle.setAttribute('aria-expanded', 'true');
    navToggle.setAttribute('aria-label', 'Close menu');
    navToggle.classList.add('is-active');
    navPanel.classList.add('is-open');
    document.body.classList.add('no-scroll');
  }

  function closePanel(focusToggle) {
    if (!panelOpen || !navToggle || !navPanel) {
      return;
    }

    panelOpen = false;
    navToggle.setAttribute('aria-expanded', 'false');
    navToggle.setAttribute('aria-label', 'Open menu');
    navToggle.classList.remove('is-active');
    navPanel.classList.remove('is-open');
    document.body.classList.remove('no-scroll');
    collapseMobileSubmenus();

    if (focusToggle) {
      navToggle.focus();
    }
  }

  if (navToggle && navPanel) {
    navToggle.addEventListener('click', function () {
      if (panelOpen) {
        closePanel(false);
      } else {
        openPanel();
      }
    });
  }

  // --------------------------------------------------------------------
  // Dropdown Menus (Corporate / People First)
  // --------------------------------------------------------------------

  var dropdownRoots = Array.prototype.slice.call(document.querySelectorAll('.nav-item-dropdown'));

  var instances = dropdownRoots.map(function (root) {
    var trigger = root.querySelector('.nav-link--parent');
    var menu = root.querySelector('.nav-dropdown');
    var items = Array.prototype.slice.call(menu.querySelectorAll('[role="menuitem"]'));

    return {
      root: root,
      trigger: trigger,
      menu: menu,
      items: items,
      open: false,
      // "activated" distinguishes a hover preview from an explicit
      // click/keyboard activation. A real pointer click always fires
      // mouseenter immediately before click, so without this flag the
      // click handler would see the dropdown already hover-opened and
      // immediately toggle it closed again.
      activated: false,
      closeTimer: null
    };
  });

  var openInstance = null;

  function focusItem(instance, index) {
    var count = instance.items.length;
    var normalized = ((index % count) + count) % count;
    instance.items[normalized].focus();
  }

  function openDropdown(instance, options) {
    if (!isDesktop()) {
      return;
    }

    options = options || {};

    if (!instance.open) {
      if (openInstance && openInstance !== instance) {
        closeDropdown(openInstance, false);
      }

      instance.open = true;
      instance.trigger.setAttribute('aria-expanded', 'true');
      instance.menu.classList.add('is-open');
      openInstance = instance;
    }

    if (options.activate) {
      instance.activated = true;
    }

    if (typeof options.focusIndex === 'number') {
      focusItem(instance, options.focusIndex);
    }
  }

  function closeDropdown(instance, focusTrigger) {
    if (!instance.open) {
      return;
    }

    instance.open = false;
    instance.activated = false;
    instance.trigger.setAttribute('aria-expanded', 'false');
    instance.menu.classList.remove('is-open');

    if (openInstance === instance) {
      openInstance = null;
    }

    if (focusTrigger) {
      instance.trigger.focus();
    }
  }

  function closeAllDropdowns() {
    instances.forEach(function (instance) {
      closeDropdown(instance, false);
    });
  }

  // ---- Mobile accordion variant ----
  // Below the desktop breakpoint the same trigger/menu pair behaves as a
  // simple disclosure widget instead of an ARIA menu: no roving focus, no
  // hover. Items are only reachable via Tab while their submenu is open —
  // tabindex is flipped alongside the open state so keyboard users aren't
  // left unable to reach About Us / Our Values / KVKK / Career / HR Policy.

  function setMobileSubmenuOpen(instance, shouldOpen) {
    instance.menu.classList.toggle('is-open', shouldOpen);
    instance.trigger.setAttribute('aria-expanded', shouldOpen ? 'true' : 'false');
    instance.items.forEach(function (item) {
      item.setAttribute('tabindex', shouldOpen ? '0' : '-1');
    });
  }

  function collapseMobileSubmenus() {
    instances.forEach(function (instance) {
      setMobileSubmenuOpen(instance, false);
    });
  }

  function toggleMobileSubmenu(instance) {
    var willOpen = !instance.menu.classList.contains('is-open');

    // Accordion behaviour: opening one submenu collapses the other, so
    // the panel never grows taller than one expanded section at a time.
    instances.forEach(function (other) {
      if (other !== instance) {
        setMobileSubmenuOpen(other, false);
      }
    });

    setMobileSubmenuOpen(instance, willOpen);
  }

  instances.forEach(function (instance) {
    instance.trigger.addEventListener('click', function () {
      if (!isDesktop()) {
        toggleMobileSubmenu(instance);
        return;
      }

      // A hover-opened-but-not-activated menu is "confirmed" by the
      // click (focus moves in) rather than closed by it.
      if (instance.open && instance.activated) {
        closeDropdown(instance, true);
      } else {
        openDropdown(instance, { focusIndex: 0, activate: true });
      }
    });

    // Delegated keydown for both the trigger and its menu items, since
    // focus moves between them as the user navigates the open menu.
    // Desktop only — the mobile accordion has no arrow-key roving.
    instance.root.addEventListener('keydown', function (event) {
      if (!isDesktop()) {
        return;
      }

      var target = event.target;
      var isTrigger = target === instance.trigger;
      var itemIndex = instance.items.indexOf(target);
      var isItem = itemIndex !== -1;

      if (!isTrigger && !isItem) {
        return;
      }

      switch (event.key) {
        case 'ArrowDown':
          event.preventDefault();
          if (isTrigger) {
            openDropdown(instance, { focusIndex: 0, activate: true });
          } else {
            focusItem(instance, itemIndex + 1);
          }
          break;

        case 'ArrowUp':
          event.preventDefault();
          if (isTrigger) {
            openDropdown(instance, { focusIndex: instance.items.length - 1, activate: true });
          } else {
            focusItem(instance, itemIndex - 1);
          }
          break;

        case 'Home':
          if (isItem) {
            event.preventDefault();
            focusItem(instance, 0);
          }
          break;

        case 'End':
          if (isItem) {
            event.preventDefault();
            focusItem(instance, instance.items.length - 1);
          }
          break;

        default:
          break;
      }
    });

    // Hover opens/closes the dropdown but never moves focus — only an
    // explicit activation (click or keyboard) does that.
    instance.root.addEventListener('mouseenter', function () {
      if (!isDesktop()) {
        return;
      }

      window.clearTimeout(instance.closeTimer);
      openDropdown(instance);
    });

    instance.root.addEventListener('mouseleave', function () {
      if (!isDesktop()) {
        return;
      }

      instance.closeTimer = window.setTimeout(function () {
        // If keyboard focus is inside the menu, leave it open — a stray
        // hover-leave must not fight keyboard navigation.
        if (instance.root.contains(document.activeElement)) {
          return;
        }
        closeDropdown(instance, false);
      }, 150);
    });
  });

  // Clicking anywhere outside the open dropdown closes it; clicking
  // outside the open mobile panel closes that too.
  document.addEventListener('click', function (event) {
    if (panelOpen && !navPanel.contains(event.target) && !navToggle.contains(event.target)) {
      closePanel(false);
    }

    if (!openInstance) {
      return;
    }

    if (!openInstance.root.contains(event.target)) {
      closeDropdown(openInstance, false);
    }
  });

  // Tabbing focus out of the open dropdown closes it (no focus stolen —
  // the browser has already moved focus to the next element). Desktop
  // only: the mobile accordion is expected to stay open while the user
  // tabs through the rest of the panel.
  document.addEventListener('focusin', function (event) {
    if (!openInstance) {
      return;
    }

    if (!openInstance.root.contains(event.target)) {
      closeDropdown(openInstance, false);
    }
  });

  document.addEventListener('keydown', function (event) {
    if (event.key !== 'Escape') {
      return;
    }

    if (!isDesktop()) {
      var openMobileInstance = instances.filter(function (instance) {
        return instance.menu.classList.contains('is-open');
      })[0];

      if (openMobileInstance) {
        setMobileSubmenuOpen(openMobileInstance, false);
        openMobileInstance.trigger.focus();
        return;
      }

      if (panelOpen) {
        closePanel(true);
        return;
      }
    }

    if (!openInstance) {
      return;
    }

    var hadFocusInside = openInstance.root.contains(document.activeElement);
    closeDropdown(openInstance, hadFocusInside);
  });

  // If the viewport crosses the desktop breakpoint, reset both the
  // desktop dropdown state and the mobile panel/accordion state rather
  // than leaving a mismatched aria-expanded on either.
  desktopQuery.addEventListener('change', function () {
    closeAllDropdowns();
    collapseMobileSubmenus();
    closePanel(false);
  });
})();


// ==========================================================================
// Scroll Reveal (Intersection Observer)
// Milestone 4 · Commit 5 — Company Timeline. Generic, reusable utility
// for docs/05_Animations.md's "Scroll Animations": fades/animates any
// [data-reveal] element into view the first time it enters the
// viewport, then stops observing it (no repeated animation on re-scroll).
// CSS lives in site.css Section 18 — this file only decides *when* to
// add .is-visible; the hidden starting state itself is gated behind the
// .js class (see _Layout.cshtml), so content stays visible by default if
// this script never runs.
// ==========================================================================

(function () {
  'use strict';

  // [data-reveal-group] children get a --reveal-index so Section 18's
  // transition-delay: calc(var(--reveal-index, 0) * 90ms) staggers them
  // — used by the Timeline's items, reusable by any future grouped list.
  var revealGroups = Array.prototype.slice.call(document.querySelectorAll('[data-reveal-group]'));

  revealGroups.forEach(function (group) {
    Array.prototype.slice.call(group.children).forEach(function (child, index) {
      child.style.setProperty('--reveal-index', index);
    });
  });

  var revealTargets = Array.prototype.slice.call(document.querySelectorAll('[data-reveal]'));

  if (!revealTargets.length) {
    return;
  }

  if (!('IntersectionObserver' in window)) {
    // No graceful per-element animation available — reveal immediately
    // rather than leaving content waiting on a scroll event that will
    // never come.
    revealTargets.forEach(function (target) {
      target.classList.add('is-visible');
    });
    return;
  }

  var revealObserver = new IntersectionObserver(function (entries, observer) {
    entries.forEach(function (entry) {
      if (!entry.isIntersecting) {
        return;
      }

      entry.target.classList.add('is-visible');
      observer.unobserve(entry.target);
    });
  }, { threshold: 0.2, rootMargin: '0px 0px -10% 0px' });

  revealTargets.forEach(function (target) {
    revealObserver.observe(target);
  });
})();


// ==========================================================================
// Video Showcase — standard video player modal
// Milestone 4 · Commit 5 — Company Timeline. Pairs every
// [data-video-trigger] with the modal it names via aria-controls (see
// Views/Shared/_VideoShowcase.cshtml for why they're markup siblings
// rather than nested) and drives open/close, a focus trap, Escape-to-
// close and pausing the video on close so audio never keeps playing
// behind a closed dialog. Reusable as-is if more than one Video Showcase
// ends up on the same page.
// ==========================================================================

(function () {
  'use strict';

  var triggers = Array.prototype.slice.call(document.querySelectorAll('[data-video-trigger]'));

  triggers.forEach(function (trigger) {
    var modalId = trigger.getAttribute('aria-controls');
    var modal = modalId ? document.getElementById(modalId) : null;

    if (!modal) {
      return;
    }

    var closers = Array.prototype.slice.call(modal.querySelectorAll('[data-video-close]'));
    var videoElement = modal.querySelector('[data-video-element]');
    var lastFocused = null;

    function focusableElements() {
      return Array.prototype.slice.call(
        modal.querySelectorAll('button, [href], video[controls], input, select, textarea')
      ).filter(function (el) {
        return !el.hasAttribute('disabled');
      });
    }

    function onKeydown(event) {
      if (event.key === 'Escape') {
        closeModal();
        return;
      }

      if (event.key !== 'Tab') {
        return;
      }

      var focusable = focusableElements();
      if (!focusable.length) {
        return;
      }

      var first = focusable[0];
      var last = focusable[focusable.length - 1];

      if (event.shiftKey && document.activeElement === first) {
        event.preventDefault();
        last.focus();
      } else if (!event.shiftKey && document.activeElement === last) {
        event.preventDefault();
        first.focus();
      }
    }

    function openModal() {
      lastFocused = document.activeElement;
      modal.hidden = false;
      document.body.classList.add('no-scroll');
      document.addEventListener('keydown', onKeydown);

      var closeButton = modal.querySelector('.video-modal-close');
      if (closeButton) {
        closeButton.focus();
      }
    }

    function closeModal() {
      modal.hidden = true;
      document.body.classList.remove('no-scroll');
      document.removeEventListener('keydown', onKeydown);

      if (videoElement) {
        videoElement.pause();
        videoElement.currentTime = 0;
      }

      if (lastFocused && typeof lastFocused.focus === 'function') {
        lastFocused.focus();
      }
    }

    trigger.addEventListener('click', openModal);
    closers.forEach(function (closer) {
      closer.addEventListener('click', closeModal);
    });
  });
})();


// ==========================================================================
// History Info Panel ↔ Carousel sync
// Company History Section. _HistoryInfoPanel (Prev/Next only) and
// _HistoryCarousel (card carousel) are paired at runtime by a shared
// data-history-* id: clicking Prev/Next scrolls the matching card into
// view, and scrolling the carousel (touch, Previous/Next, or native
// drag) keeps the buttons' disabled state in sync with scroll position.
// An IntersectionObserver on the scroll track decides which card counts
// as "active" rather than tracking scroll position by hand.
// ==========================================================================

(function () {
  'use strict';

  var carousels = Array.prototype.slice.call(document.querySelectorAll('[data-history-carousel]'));

  carousels.forEach(function (carousel) {
    var id = carousel.getAttribute('data-history-carousel');
    var infoPanel = id ? document.querySelector('[data-history-info-panel="' + id + '"]') : null;
    var track = carousel.querySelector('[data-history-track]');
    var cards = Array.prototype.slice.call(carousel.querySelectorAll('[data-history-index]'));
    var prevBtn = infoPanel ? infoPanel.querySelector('[data-history-prev]') : null;
    var nextBtn = infoPanel ? infoPanel.querySelector('[data-history-next]') : null;

    if (!track || !cards.length) {
      return;
    }

    var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    var activeIndex = 0;

    function updateNavState() {
      if (prevBtn) {
        prevBtn.disabled = track.scrollLeft <= 1;
      }
      if (nextBtn) {
        nextBtn.disabled = track.scrollLeft >= track.scrollWidth - track.clientWidth - 1;
      }
    }

    function setActive(index) {
      if (index === activeIndex) {
        return;
      }
      activeIndex = index;

      cards.forEach(function (card, i) {
        card.classList.toggle('is-active', i === index);
      });
    }

    // True while an explicit navigation (year click / Prev / Next) is
    // driving the scroll. At the very start/end of the track two cards
    // can be equally (or near-equally) visible — e.g. clicking the last
    // year can't scroll it flush to the leading edge, so it ties with
    // its neighbour on intersection ratio. Suppressing the ratio-based
    // auto-detect for the duration of that scroll keeps the explicitly
    // chosen card authoritative; ratio-based detection resumes
    // afterwards for organic touch/drag scrolling.
    var isProgrammaticScroll = false;
    var programmaticScrollTimer = null;

    function scrollToCard(index) {
      var card = cards[index];
      if (!card) {
        return;
      }

      setActive(index);

      isProgrammaticScroll = true;
      window.clearTimeout(programmaticScrollTimer);
      programmaticScrollTimer = window.setTimeout(function () {
        isProgrammaticScroll = false;
      }, reducedMotion ? 50 : 700);

      card.scrollIntoView({
        behavior: reducedMotion ? 'auto' : 'smooth',
        inline: 'start',
        block: 'nearest'
      });
    }

    if (prevBtn) {
      prevBtn.addEventListener('click', function () {
        scrollToCard(Math.max(activeIndex - 1, 0));
      });
    }

    if (nextBtn) {
      nextBtn.addEventListener('click', function () {
        scrollToCard(Math.min(activeIndex + 1, cards.length - 1));
      });
    }

    if ('IntersectionObserver' in window) {
      // Wide viewports can show more than one card past the 60%
      // threshold at once (e.g. three 340px cards fit a 1108px track).
      // Picking "whichever entry the callback happened to report last"
      // would jump to the trailing card instead of the leading one, so
      // ratios are tracked per card and the single highest wins.
      var ratios = cards.map(function () {
        return 0;
      });

      function pickActiveFromRatios() {
        var bestIndex = -1;
        var bestRatio = 0;

        ratios.forEach(function (ratio, i) {
          if (ratio > bestRatio) {
            bestRatio = ratio;
            bestIndex = i;
          }
        });

        if (bestIndex !== -1 && bestRatio >= 0.6) {
          setActive(bestIndex);
        }
      }

      var observer = new IntersectionObserver(function (entries) {
        entries.forEach(function (entry) {
          var index = cards.indexOf(entry.target);
          if (index !== -1) {
            ratios[index] = entry.intersectionRatio;
          }
        });

        if (!isProgrammaticScroll) {
          pickActiveFromRatios();
        }
      }, { root: track, threshold: [0, 0.15, 0.3, 0.45, 0.6, 0.75, 0.9, 1] });

      cards.forEach(function (card) {
        observer.observe(card);
      });
    }

    track.addEventListener('scroll', function () {
      window.requestAnimationFrame(updateNavState);
    });

    updateNavState();
  });
})();


// ==========================================================================
// Projects Showcase slider
// Home page, directly below Company History. Embla Carousel (+ its
// Autoplay plugin) drives panning, infinite loop and touch drag — vendored
// at wwwroot/lib/embla-carousel, script tags loaded Home-page-only from
// Views/Home/Index.cshtml — so this only wires Embla up to the section's
// Previous/Next buttons and to Autoplay's pause/resume. Since the vendor
// scripts aren't present on every page this file runs on, bail out
// up front if EmblaCarousel never loaded.
//
// 2026-07-28 refinement: replaces the previous native scroll-snap track
// (overflow-x: auto + scroll-snap-type), which is also what fixed that
// track occasionally capturing the page's vertical scroll — Embla's
// viewport is overflow: hidden and pans via transform instead, so there
// is no horizontal overflow:auto element for wheel/touch events to be
// funneled into.
// ==========================================================================

(function () {
  'use strict';

  if (typeof window.EmblaCarousel !== 'function') {
    return;
  }

  var sliders = Array.prototype.slice.call(document.querySelectorAll('[data-projects-slider]'));

  sliders.forEach(function (slider) {
    var viewport = slider.querySelector('[data-projects-viewport]');
    var prevBtn = slider.querySelector('[data-projects-prev]');
    var nextBtn = slider.querySelector('[data-projects-next]');

    if (!viewport) {
      return;
    }

    var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
    var plugins = [];

    // No Autoplay at all under prefers-reduced-motion, rather than an
    // instant/no-transition version of it — matches this file's existing
    // reduced-motion convention (e.g. the History Carousel above) of
    // leaving motion-driven behaviour off entirely instead of faking it.
    if (!reducedMotion && typeof window.EmblaCarouselAutoplay === 'function') {
      plugins.push(
        window.EmblaCarouselAutoplay({
          delay: 5500,
          stopOnInteraction: false,
          stopOnMouseEnter: true
        })
      );
    }

    var emblaApi = window.EmblaCarousel(viewport, { loop: true, align: 'start', slidesToScroll: 1 }, plugins);
    var autoplay = emblaApi.plugins().autoplay;

    // Prev/Next don't fire Embla's own pointerDown, so Autoplay's built-in
    // stopOnInteraction/stopOnMouseEnter handling never sees a button
    // click — reset() restarts the delay timer (rather than stop(), since
    // stopOnInteraction: false above means autoplay should keep running,
    // just without immediately advancing right after a manual click).
    function resetAutoplay() {
      if (autoplay) {
        autoplay.reset();
      }
    }

    // loop: true means there is always a valid prev/next slide, so unlike
    // the History Carousel's buttons above, these are never disabled.
    if (prevBtn) {
      prevBtn.addEventListener('click', function () {
        emblaApi.scrollPrev();
        resetAutoplay();
      });
    }

    if (nextBtn) {
      nextBtn.addEventListener('click', function () {
        emblaApi.scrollNext();
        resetAutoplay();
      });
    }
  });
})();
