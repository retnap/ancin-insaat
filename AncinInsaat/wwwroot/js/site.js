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
    navToggle.setAttribute('aria-label', 'Menüyü kapat');
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
    navToggle.setAttribute('aria-label', 'Menüyü aç');
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
// view via scrollIntoView, and the track's own 'scroll' event (fired for
// that programmatic scroll same as any other) keeps the buttons'
// disabled state in sync. Prev/Next are the only way to move the track —
// site.css's History Carousel section sets the track to overflow: hidden
// specifically so wheel, trackpad, touch-swipe and drag input can't
// (2026-08-03), leaving scrollIntoView as the sole driver. An
// IntersectionObserver on the track decides which card counts as
// "active" rather than tracking scroll position by hand.
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
    // afterwards (2026-08-03: now only reachable via Prev/Next, since
    // the track no longer accepts touch/drag/wheel scrolling at all).
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
// Home Timeline (Zaman Tüneli) devamı/gizle toggle
// Home-only _HomeTimelineCarousel cards. Each card's [data-timeline-toggle]
// button independently shows/hides that card's .home-timeline-more block
// (site.css animates the reveal via grid-template-rows) and swaps its own
// label between "devamı..." and "<- gizle" — unrelated to the History Info
// Panel sync above, and never touches About Us's shared _HistoryCarousel
// cards, which have no [data-timeline-toggle] markup.
// ==========================================================================

(function () {
  'use strict';

  var toggles = Array.prototype.slice.call(document.querySelectorAll('[data-timeline-toggle]'));

  toggles.forEach(function (toggle) {
    var card = toggle.closest('.home-timeline-card');
    if (!card) {
      return;
    }

    toggle.addEventListener('click', function () {
      var expanded = card.classList.toggle('is-expanded');
      toggle.setAttribute('aria-expanded', expanded ? 'true' : 'false');
      toggle.textContent = expanded ? '<- gizle' : 'devamı...';
    });
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
          delay: 3500,
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


// ==========================================================================
// Projects Listing — Category Tabs + Filter Dropdowns (client-side filter)
// Combines the status Category Tabs with the Project Type / Location
// Filter Dropdowns into a single AND-filter over the Projects Grid, without
// a page navigation — docs/01_SiteMap.md: "Filtering should happen without
// navigating to another page." Toggles [hidden] on non-matching
// .projects-grid-item cards rather than removing them from the DOM, so any
// combination of the three filters is instant with no re-fetch or re-render.
// ==========================================================================

(function () {
  'use strict';

  var grid = document.querySelector('[data-project-grid]');

  if (!grid) {
    return;
  }

  var gridItems = Array.prototype.slice.call(grid.querySelectorAll('[data-project-status]'));
  var emptyMessage = grid.parentElement ? grid.parentElement.querySelector('[data-project-filter-empty]') : null;

  var state = { status: 'all', type: 'all', location: 'all' };

  function matchesState(item) {
    var statusOk = state.status === 'all' || item.getAttribute('data-project-status') === state.status;
    var typeOk = state.type === 'all' || item.getAttribute('data-project-type') === state.type;
    var locationOk = state.location === 'all' || item.getAttribute('data-project-location') === state.location;

    return statusOk && typeOk && locationOk;
  }

  function applyFilters() {
    var visibleCount = 0;

    gridItems.forEach(function (item) {
      var isMatch = matchesState(item);
      item.hidden = !isMatch;

      if (isMatch) {
        visibleCount += 1;
      }
    });

    if (emptyMessage) {
      emptyMessage.hidden = visibleCount > 0;
    }
  }

  // ---- Category Tabs ----

  var tabGroup = document.querySelector('[data-project-filter-group]');

  if (tabGroup) {
    var tabButtons = Array.prototype.slice.call(tabGroup.querySelectorAll('[data-project-filter]'));

    tabButtons.forEach(function (button) {
      button.addEventListener('click', function () {
        if (button.getAttribute('aria-pressed') === 'true') {
          return;
        }

        tabButtons.forEach(function (otherButton) {
          otherButton.setAttribute('aria-pressed', 'false');
          otherButton.classList.remove('is-active');
        });

        button.setAttribute('aria-pressed', 'true');
        button.classList.add('is-active');

        state.status = button.getAttribute('data-project-filter');
        applyFilters();
      });
    });
  }

  // ---- Filter Dropdowns (Project Type / Location) ----
  // Custom listbox popup (WAI-ARIA Listbox pattern), following the same
  // button+popup/arrow-key-roving shape as the Navbar's dropdown menu.

  var dropdownRoots = Array.prototype.slice.call(document.querySelectorAll('[data-filter-dropdown]'));

  var instances = dropdownRoots.map(function (root) {
    var trigger = root.querySelector('.filter-dropdown-trigger');
    var valueEl = root.querySelector('[data-filter-dropdown-value]');
    var menu = root.querySelector('.filter-dropdown-menu');
    var options = Array.prototype.slice.call(menu.querySelectorAll('[role="option"]'));

    return {
      root: root,
      trigger: trigger,
      valueEl: valueEl,
      menu: menu,
      options: options,
      filterKey: root.getAttribute('data-filter-key'),
      open: false
    };
  });

  var openInstance = null;

  function focusOption(instance, index) {
    var count = instance.options.length;
    var normalized = ((index % count) + count) % count;

    instance.options.forEach(function (option, i) {
      option.setAttribute('tabindex', i === normalized ? '0' : '-1');
    });
    instance.options[normalized].focus();
  }

  function openMenu(instance) {
    if (instance.open) {
      return;
    }

    if (openInstance && openInstance !== instance) {
      closeMenu(openInstance, false);
    }

    instance.open = true;
    instance.trigger.setAttribute('aria-expanded', 'true');
    instance.menu.classList.add('is-open');
    openInstance = instance;

    var selectedIndex = instance.options.findIndex(function (option) {
      return option.getAttribute('aria-selected') === 'true';
    });
    focusOption(instance, selectedIndex === -1 ? 0 : selectedIndex);
  }

  function closeMenu(instance, focusTrigger) {
    if (!instance.open) {
      return;
    }

    instance.open = false;
    instance.trigger.setAttribute('aria-expanded', 'false');
    instance.menu.classList.remove('is-open');

    if (openInstance === instance) {
      openInstance = null;
    }

    if (focusTrigger) {
      instance.trigger.focus();
    }
  }

  function selectOption(instance, option) {
    instance.options.forEach(function (o) {
      o.setAttribute('aria-selected', 'false');
    });
    option.setAttribute('aria-selected', 'true');
    instance.valueEl.textContent = option.textContent;
    closeMenu(instance, true);

    state[instance.filterKey] = option.getAttribute('data-filter-value');
    applyFilters();
  }

  instances.forEach(function (instance) {
    instance.trigger.addEventListener('click', function () {
      if (instance.open) {
        closeMenu(instance, true);
      } else {
        openMenu(instance);
      }
    });

    instance.options.forEach(function (option) {
      option.addEventListener('click', function () {
        selectOption(instance, option);
      });
    });

    instance.root.addEventListener('keydown', function (event) {
      var target = event.target;
      var isTrigger = target === instance.trigger;
      var optionIndex = instance.options.indexOf(target);
      var isOption = optionIndex !== -1;

      if (!isTrigger && !isOption) {
        return;
      }

      switch (event.key) {
        case 'ArrowDown':
          event.preventDefault();
          if (!instance.open) {
            openMenu(instance);
          } else {
            focusOption(instance, optionIndex + 1);
          }
          break;

        case 'ArrowUp':
          event.preventDefault();
          if (!instance.open) {
            openMenu(instance);
          } else {
            focusOption(instance, optionIndex - 1);
          }
          break;

        case 'Home':
          if (isOption) {
            event.preventDefault();
            focusOption(instance, 0);
          }
          break;

        case 'End':
          if (isOption) {
            event.preventDefault();
            focusOption(instance, instance.options.length - 1);
          }
          break;

        case 'Enter':
        case ' ':
          if (isOption) {
            event.preventDefault();
            selectOption(instance, target);
          }
          break;

        case 'Escape':
          if (instance.open) {
            event.preventDefault();
            closeMenu(instance, true);
          }
          break;

        default:
          break;
      }
    });
  });

  document.addEventListener('click', function (event) {
    if (!openInstance) {
      return;
    }

    if (!openInstance.root.contains(event.target)) {
      closeMenu(openInstance, false);
    }
  });

  document.addEventListener('focusin', function (event) {
    if (!openInstance) {
      return;
    }

    if (!openInstance.root.contains(event.target)) {
      closeMenu(openInstance, false);
    }
  });
})();


// ==========================================================================
// Media Viewer (Gallery Lightbox / Floor Plan Viewer)
// One shared full-viewport dialog (_MediaViewer.cshtml) reused by any
// number of thumbnail groups sharing a data-media-viewer-group key (see
// _ProjectGallery.cshtml, _FloorPlans.cshtml — "gallery" and "floorplans").
// Previous/Next wrap around rather than disabling at the ends. Follows the
// same focus-trap / Escape-to-close / body-scroll-lock shape as the Video
// Showcase modal (above) but generalised across an ordered item list
// instead of a single video, plus swipe navigation for touch.
// ==========================================================================

(function () {
  'use strict';

  var viewer = document.querySelector('[data-media-viewer]');
  var triggers = Array.prototype.slice.call(document.querySelectorAll('[data-media-viewer-trigger]'));

  if (!viewer || !triggers.length) {
    return;
  }

  var imageEl = viewer.querySelector('[data-media-viewer-image]');
  var counterEl = viewer.querySelector('[data-media-viewer-counter]');
  var prevBtn = viewer.querySelector('[data-media-viewer-prev]');
  var nextBtn = viewer.querySelector('[data-media-viewer-next]');
  var closeBtn = viewer.querySelector('.media-viewer-close');
  var closers = Array.prototype.slice.call(viewer.querySelectorAll('[data-media-viewer-close]'));
  var panelEl = viewer.querySelector('.media-viewer-panel');

  // ---- Zoom / Pan / Fullscreen (La Fiore Karabağ 2. Etap Vaziyet Planı/
  // Concept/Gallery phase, 2026-08-09) — upgrades this one shared instance
  // in place, so every existing and future trigger group (Gallery, Floor
  // Plans, Concept, Site Plan) gains these capabilities automatically. ----
  var figureEl = viewer.querySelector('.media-viewer-figure');
  var zoomOutBtn = viewer.querySelector('[data-media-viewer-zoom-out]');
  var zoomInBtn = viewer.querySelector('[data-media-viewer-zoom-in]');
  var fullscreenBtn = viewer.querySelector('[data-media-viewer-fullscreen]');
  var MIN_SCALE = 1;
  var MAX_SCALE = 6;
  var ZOOM_STEP = 0.5;
  // Click (desktop) / double-tap (mobile) zoom toggle (La Fiore Karabağ 2.
  // Etap Media Viewer improvements, 2026-08-09) — jumps to a fixed, moderate
  // scale rather than MAX_SCALE, so the toolbar/wheel/pinch are still the
  // way to reach full 6x detail; click/double-tap again to reset to 1x.
  var CLICK_ZOOM_SCALE = 3;
  var TAP_MOVE_THRESHOLD = 10;
  var DOUBLE_TAP_INTERVAL_MS = 350;
  var dragState = null;
  var dragMoved = false;
  var pinchStartDistance = null;
  var pinchStartScale = 1;
  var panStart = null;
  var singleTouchStart = null;
  var suppressClickUntil = 0;
  var lastTapTime = 0;
  var lastTapX = 0;
  var lastTapY = 0;

  // Group membership is resolved from each trigger's data-media-viewer-group
  // value at click time rather than baked into a fixed map at page load —
  // the Project Gallery's category filter (above) repoints that attribute
  // between "gallery-all" and a per-category group as the user switches
  // filters, and Prev/Next must always reflect whichever list was showing
  // when the viewer was opened.
  triggers.forEach(function (trigger) {
    trigger.addEventListener('click', function () {
      var groupKey = trigger.getAttribute('data-media-viewer-group') || 'default';
      var groupTriggers = triggers.filter(function (candidate) {
        return (candidate.getAttribute('data-media-viewer-group') || 'default') === groupKey;
      });

      open(groupTriggers, groupTriggers.indexOf(trigger), trigger);
    });
  });

  var state = { items: [], index: 0, scale: 1, translateX: 0, translateY: 0 };
  var lastFocused = null;
  var touchStartX = null;
  var touchStartY = null;

  function itemCount() {
    return state.items.length;
  }

  function applyTransform(animate) {
    imageEl.style.transition = animate ? '' : 'none';
    imageEl.style.transform = 'translate(' + state.translateX + 'px, ' + state.translateY + 'px) scale(' + state.scale + ')';
    imageEl.classList.toggle('is-zoomed', state.scale > MIN_SCALE);
    if (zoomOutBtn) {
      zoomOutBtn.disabled = state.scale <= MIN_SCALE;
    }
    if (zoomInBtn) {
      zoomInBtn.disabled = state.scale >= MAX_SCALE;
    }
  }

  function resetZoom() {
    state.scale = MIN_SCALE;
    state.translateX = 0;
    state.translateY = 0;
    applyTransform(false);
  }

  function setScale(nextScale) {
    nextScale = Math.min(MAX_SCALE, Math.max(MIN_SCALE, nextScale));

    if (nextScale === MIN_SCALE) {
      state.translateX = 0;
      state.translateY = 0;
    }

    state.scale = nextScale;
    applyTransform(true);
  }

  function zoomIn() {
    setScale(state.scale + ZOOM_STEP);
  }

  function zoomOut() {
    setScale(state.scale - ZOOM_STEP);
  }

  function toggleFullscreen() {
    if (document.fullscreenElement) {
      document.exitFullscreen();
    } else if (viewer.requestFullscreen) {
      viewer.requestFullscreen();
    }
  }

  function render() {
    var item = state.items[state.index];

    imageEl.classList.remove('is-loaded');
    imageEl.onload = function () {
      imageEl.classList.add('is-loaded');
    };
    imageEl.src = item.getAttribute('data-media-viewer-src');
    imageEl.alt = item.getAttribute('data-media-viewer-alt') || '';

    counterEl.textContent = (state.index + 1) + ' / ' + state.items.length;

    var hasMultiple = state.items.length > 1;
    prevBtn.hidden = !hasMultiple;
    nextBtn.hidden = !hasMultiple;

    resetZoom();
  }

  function focusableElements() {
    return Array.prototype.slice.call(viewer.querySelectorAll('button')).filter(function (el) {
      return !el.hidden && !el.disabled;
    });
  }

  function onKeydown(event) {
    if (event.key === 'Escape') {
      close();
      return;
    }

    if (event.key === 'ArrowLeft') {
      showPrev();
      return;
    }

    if (event.key === 'ArrowRight') {
      showNext();
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

  function open(items, index, trigger) {
    state.items = items;
    state.index = index;
    lastFocused = trigger || document.activeElement;

    render();
    viewer.hidden = false;
    document.body.classList.add('no-scroll');
    document.addEventListener('keydown', onKeydown);

    if (closeBtn) {
      closeBtn.focus();
    }
  }

  function close() {
    if (document.fullscreenElement) {
      document.exitFullscreen();
    }

    viewer.hidden = true;
    document.body.classList.remove('no-scroll');
    document.removeEventListener('keydown', onKeydown);
    resetZoom();

    if (lastFocused && typeof lastFocused.focus === 'function') {
      lastFocused.focus();
    }
  }

  function showPrev() {
    var count = itemCount();
    if (count < 2) {
      return;
    }
    state.index = (state.index - 1 + count) % count;
    render();
  }

  function showNext() {
    var count = itemCount();
    if (count < 2) {
      return;
    }
    state.index = (state.index + 1) % count;
    render();
  }

  closers.forEach(function (closer) {
    closer.addEventListener('click', close);
  });

  // Backdrop click-to-close (2026-09-06) — .media-viewer-panel is a
  // position:relative box sized to the full viewport (100%/100%) so its
  // flex centering can lay out the figure/toolbar/nav/counter; that means
  // clicks on the empty area around the image land on the panel itself,
  // not on .media-viewer-backdrop underneath it (data-media-viewer-close
  // alone never fires for them). Closing only when the click target is the
  // panel element itself — never a descendant — means clicks on the image,
  // toolbar, nav arrows and close button (all descendants) are unaffected;
  // this is the shared instance every trigger group (Gallery, Floor Plans,
  // Concept, Site Plan/Vaziyet Planı) reuses, so the fix applies everywhere
  // at once with no per-project changes.
  if (panelEl) {
    panelEl.addEventListener('click', function (event) {
      if (event.target === panelEl) {
        close();
      }
    });
  }

  prevBtn.addEventListener('click', showPrev);
  nextBtn.addEventListener('click', showNext);

  if (zoomInBtn) {
    zoomInBtn.addEventListener('click', zoomIn);
  }
  if (zoomOutBtn) {
    zoomOutBtn.addEventListener('click', zoomOut);
  }
  if (fullscreenBtn) {
    fullscreenBtn.addEventListener('click', toggleFullscreen);
  }

  document.addEventListener('fullscreenchange', function () {
    if (!fullscreenBtn) {
      return;
    }

    var isFullscreen = document.fullscreenElement === viewer;
    fullscreenBtn.classList.toggle('is-active', isFullscreen);
    fullscreenBtn.setAttribute('aria-label', isFullscreen ? 'Tam ekrandan çık' : 'Tam ekran');
  });

  if (figureEl) {
    figureEl.addEventListener('wheel', function (event) {
      event.preventDefault();
      setScale(state.scale + (event.deltaY < 0 ? ZOOM_STEP : -ZOOM_STEP));
    }, { passive: false });
  }

  imageEl.addEventListener('mousedown', function (event) {
    if (state.scale <= MIN_SCALE) {
      return;
    }

    dragState = { startX: event.clientX, startY: event.clientY, originX: state.translateX, originY: state.translateY };
    dragMoved = false;
    imageEl.classList.add('is-dragging');
    event.preventDefault();
  });

  window.addEventListener('mousemove', function (event) {
    if (!dragState) {
      return;
    }

    var deltaX = event.clientX - dragState.startX;
    var deltaY = event.clientY - dragState.startY;
    if (Math.abs(deltaX) > TAP_MOVE_THRESHOLD || Math.abs(deltaY) > TAP_MOVE_THRESHOLD) {
      dragMoved = true;
    }

    state.translateX = dragState.originX + deltaX;
    state.translateY = dragState.originY + deltaY;
    applyTransform(false);
  });

  window.addEventListener('mouseup', function () {
    if (!dragState) {
      return;
    }

    dragState = null;
    imageEl.classList.remove('is-dragging');
  });

  // Single left click toggles zoom: in (to CLICK_ZOOM_SCALE) when at 1x, back
  // out to 1x otherwise. Guarded by dragMoved so releasing a pan drag on the
  // image doesn't also toggle the zoom level.
  imageEl.addEventListener('click', function () {
    // Touch interactions (swipe, pinch, pan-release, double-tap) can leave a
    // trailing synthetic click on mobile browsers — touch already has its
    // own double-tap toggle below, so any click arriving right after a
    // touch gesture is that synthetic echo, not a real desktop click.
    if (Date.now() < suppressClickUntil) {
      return;
    }

    if (dragMoved) {
      dragMoved = false;
      return;
    }

    setScale(state.scale > MIN_SCALE ? MIN_SCALE : CLICK_ZOOM_SCALE);
  });

  function touchDistance(touches) {
    var dx = touches[0].clientX - touches[1].clientX;
    var dy = touches[0].clientY - touches[1].clientY;
    return Math.sqrt((dx * dx) + (dy * dy));
  }

  viewer.addEventListener('touchstart', function (event) {
    if (event.touches.length === 2) {
      pinchStartDistance = touchDistance(event.touches);
      pinchStartScale = state.scale;
      touchStartX = null;
      panStart = null;
      singleTouchStart = null;
      return;
    }

    if (event.touches.length === 1) {
      var startTouch = event.touches[0];
      // Recorded regardless of the zoomed/unzoomed branch below — double-tap
      // zoom must work whether the image is currently zoomed in or not.
      singleTouchStart = { x: startTouch.clientX, y: startTouch.clientY, time: Date.now() };

      if (state.scale > MIN_SCALE) {
        panStart = { x: startTouch.clientX, y: startTouch.clientY, originX: state.translateX, originY: state.translateY };
        touchStartX = null;
      } else {
        var touch = event.changedTouches[0];
        touchStartX = touch.clientX;
        touchStartY = touch.clientY;
        panStart = null;
      }
    }
  }, { passive: true });

  viewer.addEventListener('touchmove', function (event) {
    if (event.touches.length === 2 && pinchStartDistance) {
      event.preventDefault();
      setScale(pinchStartScale * (touchDistance(event.touches) / pinchStartDistance));
      return;
    }

    if (panStart && event.touches.length === 1) {
      event.preventDefault();
      state.translateX = panStart.originX + (event.touches[0].clientX - panStart.x);
      state.translateY = panStart.originY + (event.touches[0].clientY - panStart.y);
      applyTransform(false);
    }
  }, { passive: false });

  viewer.addEventListener('touchend', function (event) {
    suppressClickUntil = Date.now() + 500;

    if (event.touches.length === 0) {
      pinchStartDistance = null;
      panStart = null;
    }

    // Double-tap zoom toggle — mirrors the desktop single-click toggle
    // above. Tracked independently of the pan/swipe state so it works both
    // zoomed in (tap to reset to 1x) and zoomed out (tap to jump to
    // CLICK_ZOOM_SCALE). Only fires on an actual tap (little movement,
    // short duration), not the end of a pinch, pan, or swipe gesture.
    if (event.touches.length === 0 && singleTouchStart) {
      var endTouch = event.changedTouches[0];
      var tapMoveDist = Math.hypot(endTouch.clientX - singleTouchStart.x, endTouch.clientY - singleTouchStart.y);
      var tapDuration = Date.now() - singleTouchStart.time;
      singleTouchStart = null;

      if (tapMoveDist < TAP_MOVE_THRESHOLD && tapDuration < 500) {
        var now = Date.now();
        var distFromLastTap = Math.hypot(endTouch.clientX - lastTapX, endTouch.clientY - lastTapY);

        if (now - lastTapTime < DOUBLE_TAP_INTERVAL_MS && distFromLastTap < 40) {
          lastTapTime = 0;
          setScale(state.scale > MIN_SCALE ? MIN_SCALE : CLICK_ZOOM_SCALE);
        } else {
          lastTapTime = now;
          lastTapX = endTouch.clientX;
          lastTapY = endTouch.clientY;
        }
      }
    }

    if (touchStartX === null) {
      return;
    }

    var touch = event.changedTouches[0];
    var deltaX = touch.clientX - touchStartX;
    var deltaY = touch.clientY - touchStartY;

    touchStartX = null;
    touchStartY = null;

    if (Math.abs(deltaX) < 40 || Math.abs(deltaX) < Math.abs(deltaY)) {
      return;
    }

    if (deltaX > 0) {
      showPrev();
    } else {
      showNext();
    }
  }, { passive: true });
})();


// ==========================================================================
// Toast (La Fiore Karabağ 2. Etap Vaziyet Planı/Concept/Gallery phase,
// 2026-08-09) — one shared bottom-center status message (_Toast.cshtml),
// triggered by any [data-toast-trigger] on the page (today only
// _ProjectCatalogue.cshtml's button when CatalogueComingSoon is true). Not a
// dialog — no focus-trap, no Esc/backdrop-close, just a fade-in/auto-
// dismiss/fade-out status message.
// ==========================================================================

(function () {
  'use strict';

  var toast = document.querySelector('[data-toast]');
  var triggers = Array.prototype.slice.call(document.querySelectorAll('[data-toast-trigger]'));

  if (!toast || !triggers.length) {
    return;
  }

  var messageEl = toast.querySelector('[data-toast-message]');
  var hideTimer = null;

  function show(message) {
    if (hideTimer) {
      clearTimeout(hideTimer);
    }

    messageEl.textContent = message;
    toast.hidden = false;

    // Force a reflow so the opacity/transform transition actually runs when
    // re-triggered back-to-back (removing hidden and adding is-visible in
    // the same tick would otherwise skip straight to the end state).
    void toast.offsetWidth;
    toast.classList.add('is-visible');

    hideTimer = window.setTimeout(function () {
      toast.classList.remove('is-visible');
      hideTimer = window.setTimeout(function () {
        toast.hidden = true;
      }, 300);
    }, 3000);
  }

  triggers.forEach(function (trigger) {
    trigger.addEventListener('click', function () {
      show(trigger.getAttribute('data-toast-message') || '');
    });
  });
})();


// ==========================================================================
// Daire Planları — Apartment Type Switcher
// Project Detail Floor Plans redesign (2026-07-31). _FloorPlans.cshtml
// server-renders one [data-floorplan-panel] per apartment type inside
// .project-floorplans-text (all but the first start [hidden]); Prev/Next
// just toggle which panel is visible, wrapping around like the Media
// Viewer above. Early-exits when a project has zero or one apartment type —
// the partial only renders the prev/next buttons when there are 2+.
//
// Visual side (La Fiore Karabağ 2. Etap Floor Plans pilot, 2026-08-06):
// _FloorPlans.cshtml now also server-renders one [data-floorplan-visual] per
// entry, same count/order as [data-floorplan-panel], so show(index) toggles
// both lists together by index. Projects with only the static placeholder
// still render exactly one [data-floorplan-visual] (never toggled, same as
// before) since panels.length < 2 always matches visuals.length < 2 too.
// ==========================================================================

(function () {
  'use strict';

  var prevBtn = document.querySelector('[data-floorplan-prev]');
  var nextBtn = document.querySelector('[data-floorplan-next]');
  var panels = Array.prototype.slice.call(document.querySelectorAll('[data-floorplan-panel]'));
  var visuals = Array.prototype.slice.call(document.querySelectorAll('[data-floorplan-visual]'));

  if (!prevBtn || !nextBtn || panels.length < 2) {
    return;
  }

  var activeIndex = 0;

  function show(index) {
    activeIndex = (index + panels.length) % panels.length;

    panels.forEach(function (panel, i) {
      panel.hidden = i !== activeIndex;
    });
    visuals.forEach(function (visual, i) {
      visual.hidden = i !== activeIndex;
    });
  }

  prevBtn.addEventListener('click', function () {
    show(activeIndex - 1);
  });

  nextBtn.addEventListener('click', function () {
    show(activeIndex + 1);
  });
})();


// ==========================================================================
// Project Gallery — Category Cards
// Gallery Category Cards redesign (2026-08-04). A single-instance version
// of the Projects listing's Filter Dropdown open/close/roving-focus
// behaviour (see the "Filter Dropdowns" module above), scoped entirely to
// #project-gallery and deliberately not sharing that module: it early-exits
// whenever [data-project-grid] is absent, so it never runs on this page and
// would leave a reused _FilterDropdown.cshtml instance here inert.
//
// _ProjectGallery.cshtml renders every gallery image exactly once, in
// category order, as a .gallery-card. "Tüm Görseller" (category === 'all')
// shows every card; picking a category shows only the cards carrying that
// category (2026-08-04 fix — "all" previously showed just one card per
// category). Each card's data-media-viewer-group is repointed here to
// "gallery-all" or back to its own data-media-viewer-group-category so the
// Media Viewer (below) opens on the matching combined-or-category list. A
// brief opacity fade (see site.css's .project-gallery-cards.is-swapping)
// covers the swap so cards don't just pop between the two sets.
// ==========================================================================

(function () {
  'use strict';

  var gallerySection = document.getElementById('project-gallery');

  if (!gallerySection) {
    return;
  }

  var wrap = gallerySection.querySelector('[data-gallery-cards]');

  if (!wrap) {
    // Exactly one named category — _ProjectGallery.cshtml renders the plain
    // fallback grid instead, with no cards or dropdown to wire up here.
    return;
  }

  var cards = Array.prototype.slice.call(wrap.querySelectorAll('.gallery-card'));
  var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;
  var swapTimer = null;

  // ---- Block / Apartment Type tiers (La Fiore Karabağ 2. Etap Gallery
  // pilot, 2026-08-06) — ONLY meaningfully active for projects whose cards
  // carry data-gallery-block/data-gallery-apartment-type; on every other
  // project blockRow/apartmentRow are both null (Razor never renders them),
  // so activeBlock/activeApartmentType stay 'all' forever and every branch
  // below behaves exactly like the original single-tier category filter.
  var blockRow = gallerySection.querySelector('[data-gallery-block-row]');
  var apartmentRow = gallerySection.querySelector('[data-gallery-apartment-row]');
  var blockChips = blockRow ? Array.prototype.slice.call(blockRow.querySelectorAll('[data-gallery-block-chip]')) : [];
  var apartmentChips = apartmentRow ? Array.prototype.slice.call(apartmentRow.querySelectorAll('[data-gallery-apartment-chip]')) : [];

  var activeCategory = 'all';
  var activeBlock = 'all';
  var activeApartmentType = 'all';

  function cardMatchesCategory(card, category) {
    return category === 'all' || card.getAttribute('data-gallery-category') === category;
  }

  // Only chips whose value is actually present among cards matching the
  // current category (and, for apartment chips, the current block too) are
  // shown — and the whole row hides itself once fewer than 2 chips remain,
  // since a single remaining choice has nothing left to filter (mirrors the
  // plain-grid fallback's "one named category → no dropdown" rule above).
  // Block chips never show under "Tüm Görseller" (La Fiore Karabağ 2. Etap
  // Vaziyet Planı/Concept/Gallery phase, 2026-08-09 — previously a block
  // whose photos spanned more than one category, e.g. this project's own
  // "A Tipi Blok" spanning both Exterior and Interior, stayed visible even
  // under "Tüm Görseller"; the picker/marquee cards are what "Tüm
  // Görseller" shows now, so a dangling chip row above them was a bug, not
  // a feature). Chips still work exactly as before inside any named
  // category (e.g. İç Mekan Görselleri).
  function updateBlockChipVisibility() {
    if (!blockRow) {
      return;
    }

    var visibleCount = 0;

    blockChips.forEach(function (chip) {
      var value = chip.getAttribute('data-gallery-block-chip');
      var present = activeCategory !== 'all' && cards.some(function (card) {
        return cardMatchesCategory(card, activeCategory) && card.getAttribute('data-gallery-block') === value;
      });
      chip.hidden = !present;
      chip.classList.toggle('is-active', present && value === activeBlock);
      chip.setAttribute('aria-pressed', present && value === activeBlock ? 'true' : 'false');
      if (present) {
        visibleCount++;
      }
    });

    blockRow.hidden = visibleCount < 2;
  }

  function updateApartmentChipVisibility() {
    if (!apartmentRow) {
      return;
    }

    // Apartment type is a third tier, one level below Block — it must never
    // surface under "Tüm Görseller" (activeCategory === 'all', where a block
    // spans both Exterior and Interior cards and apartment type doesn't
    // apply across that combined set), and within a real category it must
    // stay hidden until a Block has actually been picked.
    var showRow = activeCategory !== 'all' && activeBlock !== 'all';
    var visibleCount = 0;

    apartmentChips.forEach(function (chip) {
      var value = chip.getAttribute('data-gallery-apartment-chip');
      var present = showRow && cards.some(function (card) {
        return cardMatchesCategory(card, activeCategory)
          && card.getAttribute('data-gallery-block') === activeBlock
          && card.getAttribute('data-gallery-apartment-type') === value;
      });
      chip.hidden = !present;
      chip.classList.toggle('is-active', present && value === activeApartmentType);
      chip.setAttribute('aria-pressed', present && value === activeApartmentType ? 'true' : 'false');
      if (present) {
        visibleCount++;
      }
    });

    apartmentRow.hidden = !showRow || visibleCount < 1;
  }

  function applyFilters() {
    window.clearTimeout(swapTimer);
    wrap.classList.add('is-swapping');

    swapTimer = window.setTimeout(function () {
      cards.forEach(function (card) {
        var matches = cardMatchesCategory(card, activeCategory)
          && (activeBlock === 'all' || card.getAttribute('data-gallery-block') === activeBlock)
          && (activeApartmentType === 'all' || card.getAttribute('data-gallery-apartment-type') === activeApartmentType);

        card.hidden = !matches;

        var group;
        if (!matches) {
          group = card.getAttribute('data-media-viewer-group-category');
        } else if (activeBlock === 'all' && activeApartmentType === 'all') {
          group = activeCategory === 'all' ? 'gallery-all' : card.getAttribute('data-media-viewer-group-category');
        } else {
          group = 'gallery-filtered';
        }
        card.setAttribute('data-media-viewer-group', group);
      });
      wrap.classList.remove('is-swapping');
      wrap.scrollLeft = 0;
      updateGalleryNavState();
    }, reducedMotion ? 0 : 180);
  }

  function setCategory(category) {
    activeCategory = category;
    activeBlock = 'all';
    activeApartmentType = 'all';
    updateBlockChipVisibility();
    updateApartmentChipVisibility();
    applyFilters();
  }

  function setBlock(block) {
    activeBlock = activeBlock === block ? 'all' : block;
    activeApartmentType = 'all';

    // When a Block within a real category (never "Tüm Görseller", where
    // apartment type doesn't apply) resolves to exactly one apartment type,
    // that type is auto-selected — e.g. A Tipi Blok only ever has "4+1", so
    // picking the block should immediately reveal and apply "4+1" rather
    // than making the user pick it again. A block with 2+ types (C Tipi
    // Blok's "Sağ Tip"/"Sol Tip") is left on 'all' so both remain live.
    if (activeBlock !== 'all' && activeCategory !== 'all') {
      var availableApartmentTypes = apartmentChips
        .map(function (chip) {
          return chip.getAttribute('data-gallery-apartment-chip');
        })
        .filter(function (value) {
          return cards.some(function (card) {
            return cardMatchesCategory(card, activeCategory)
              && card.getAttribute('data-gallery-block') === activeBlock
              && card.getAttribute('data-gallery-apartment-type') === value;
          });
        });

      if (availableApartmentTypes.length === 1) {
        activeApartmentType = availableApartmentTypes[0];
      }
    }

    updateBlockChipVisibility();
    updateApartmentChipVisibility();
    applyFilters();
  }

  function setApartmentType(apartmentType) {
    activeApartmentType = activeApartmentType === apartmentType ? 'all' : apartmentType;
    updateApartmentChipVisibility();
    applyFilters();
  }

  blockChips.forEach(function (chip) {
    chip.addEventListener('click', function () {
      setBlock(chip.getAttribute('data-gallery-block-chip'));
    });
  });

  apartmentChips.forEach(function (chip) {
    chip.addEventListener('click', function () {
      setApartmentType(chip.getAttribute('data-gallery-apartment-chip'));
    });
  });

  updateBlockChipVisibility();
  updateApartmentChipVisibility();

  // ---- Prev/Next nav (2026-08-04) ----
  // Adds arrow navigation alongside the track's existing native drag/
  // swipe/wheel scrolling (untouched below) — same disabled-at-the-ends
  // approach as the History Carousel's Prev/Next (site.js above), scrolling
  // by one card's width via scrollBy the way Social Facilities' track nav
  // does, since — unlike the History Carousel's track — this one stays
  // natively scrollable rather than switching to overflow: hidden.
  var prevBtn = gallerySection.querySelector('[data-gallery-prev]');
  var nextBtn = gallerySection.querySelector('[data-gallery-next]');

  function updateGalleryNavState() {
    if (prevBtn) {
      prevBtn.disabled = wrap.scrollLeft <= 1;
    }
    if (nextBtn) {
      nextBtn.disabled = wrap.scrollLeft >= wrap.scrollWidth - wrap.clientWidth - 1;
    }
  }

  function scrollGalleryByCard(direction) {
    var card = wrap.querySelector('.gallery-card:not([hidden])');

    if (!card) {
      return;
    }

    var wrapStyle = window.getComputedStyle(wrap);
    var gap = parseFloat(wrapStyle.columnGap || wrapStyle.gap || '0') || 0;
    var distance = (card.getBoundingClientRect().width + gap) * direction;

    wrap.scrollBy({ left: distance, behavior: reducedMotion ? 'auto' : 'smooth' });
  }

  if (prevBtn) {
    prevBtn.addEventListener('click', function () {
      scrollGalleryByCard(-1);
    });
  }

  if (nextBtn) {
    nextBtn.addEventListener('click', function () {
      scrollGalleryByCard(1);
    });
  }

  wrap.addEventListener('scroll', function () {
    window.requestAnimationFrame(updateGalleryNavState);
  });

  updateGalleryNavState();

  var root = gallerySection.querySelector('[data-filter-dropdown]');

  if (!root) {
    return;
  }

  var trigger = root.querySelector('.filter-dropdown-trigger');
  var valueEl = root.querySelector('[data-filter-dropdown-value]');
  var menu = root.querySelector('.filter-dropdown-menu');
  var options = Array.prototype.slice.call(menu.querySelectorAll('[role="option"]'));
  var open = false;

  function focusOption(index) {
    var count = options.length;
    var normalized = ((index % count) + count) % count;

    options.forEach(function (option, i) {
      option.setAttribute('tabindex', i === normalized ? '0' : '-1');
    });
    options[normalized].focus();
  }

  function openMenu() {
    if (open) {
      return;
    }

    open = true;
    trigger.setAttribute('aria-expanded', 'true');
    menu.classList.add('is-open');

    var selectedIndex = options.findIndex(function (option) {
      return option.getAttribute('aria-selected') === 'true';
    });
    focusOption(selectedIndex === -1 ? 0 : selectedIndex);
  }

  function closeMenu(focusTrigger) {
    if (!open) {
      return;
    }

    open = false;
    trigger.setAttribute('aria-expanded', 'false');
    menu.classList.remove('is-open');

    if (focusTrigger) {
      trigger.focus();
    }
  }

  function selectOption(option) {
    options.forEach(function (o) {
      o.setAttribute('aria-selected', 'false');
    });
    option.setAttribute('aria-selected', 'true');
    valueEl.textContent = option.textContent;
    closeMenu(true);

    setCategory(option.getAttribute('data-filter-value'));
  }

  trigger.addEventListener('click', function () {
    if (open) {
      closeMenu(true);
    } else {
      openMenu();
    }
  });

  options.forEach(function (option) {
    option.addEventListener('click', function () {
      selectOption(option);
    });
  });

  root.addEventListener('keydown', function (event) {
    var target = event.target;
    var isTrigger = target === trigger;
    var optionIndex = options.indexOf(target);
    var isOption = optionIndex !== -1;

    if (!isTrigger && !isOption) {
      return;
    }

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        if (!open) {
          openMenu();
        } else {
          focusOption(optionIndex + 1);
        }
        break;

      case 'ArrowUp':
        event.preventDefault();
        if (!open) {
          openMenu();
        } else {
          focusOption(optionIndex - 1);
        }
        break;

      case 'Home':
        if (isOption) {
          event.preventDefault();
          focusOption(0);
        }
        break;

      case 'End':
        if (isOption) {
          event.preventDefault();
          focusOption(options.length - 1);
        }
        break;

      case 'Enter':
      case ' ':
        if (isOption) {
          event.preventDefault();
          selectOption(target);
        }
        break;

      case 'Escape':
        if (open) {
          event.preventDefault();
          closeMenu(true);
        }
        break;

      default:
        break;
    }
  });

  document.addEventListener('click', function (event) {
    if (open && !root.contains(event.target)) {
      closeMenu(false);
    }
  });

  document.addEventListener('focusin', function (event) {
    if (open && !root.contains(event.target)) {
      closeMenu(false);
    }
  });

  // ---- Category Picker (Nysa Gold Residence reference redesign,
  // 2026-08-07; static layout revision, 2026-08-28) ----
  // "Tüm Görseller" shows the static category cards below instead of the
  // merged grid. Gated entirely by the picker's presence in the DOM (only
  // rendered when the project has 1+ named categories), so this is a safe
  // no-op everywhere else.
  //
  // UI revision (2026-08-07): picking a card no longer routes through
  // selectOption/setCategory — that swapped this picker for the filtered
  // grid, which read as "another category screen" rather than browsing the
  // category's photos. Instead it opens the existing Media Viewer directly,
  // scoped to every card in that category: each matching .gallery-card
  // already carries its own stable data-media-viewer-group-category
  // (server-rendered, see _ProjectGallery.cshtml), so repointing
  // data-media-viewer-group to that same value — exactly what applyFilters()
  // above already does when a real category is chosen from the dropdown —
  // then clicking the first matching card reuses the Media Viewer's own
  // click-time group resolution (see that module) to scope Prev/Next to
  // exactly this category. activeCategory/the picker-vs-grid toggle below
  // are never touched by this, so "Tüm Görseller" stays exactly where it
  // was — the dropdown and applyFilters/setCategory above are not modified.
  var picker = gallerySection.querySelector('[data-gallery-category-picker]');

  if (picker) {
    var pickerCards = Array.prototype.slice.call(picker.querySelectorAll('[data-gallery-category-card]'));

    pickerCards.forEach(function (card) {
      card.addEventListener('click', function () {
        var label = card.getAttribute('data-gallery-category-card');
        var categoryCards = cards.filter(function (galleryCard) {
          return galleryCard.getAttribute('data-gallery-category') === label;
        });

        if (!categoryCards.length) {
          return;
        }

        var groupKey = categoryCards[0].getAttribute('data-media-viewer-group-category');
        categoryCards.forEach(function (galleryCard) {
          galleryCard.setAttribute('data-media-viewer-group', groupKey);
        });

        categoryCards[0].click();
      });
    });

    var navGroup = gallerySection.querySelector('.project-gallery-nav-group');

    var updatePickerVisibility = function () {
      var showPicker = activeCategory === 'all';
      picker.hidden = !showPicker;
      wrap.hidden = showPicker;
      if (navGroup) {
        navGroup.hidden = showPicker;
      }
    };

    // Wraps setCategory (a plain hoisted function binding, safe to
    // reassign) so every path that changes activeCategory — dropdown
    // selection or a picker card click, both already funnel through it —
    // keeps the picker/grid toggle in sync without duplicating
    // applyFilters' own logic.
    var baseSetCategory = setCategory;
    setCategory = function (category) {
      baseSetCategory(category);
      updatePickerVisibility();
    };

    updatePickerVisibility();
  }
})();


// ==========================================================================
// Social Facilities — Horizontal Scroll Controls
// Project Detail redesign (2026-07-31). Previous/Next buttons scroll the
// card track by one card's width; CSS scroll-snap (site.css Section 26)
// handles direct touch/trackpad scrolling on its own.
// ==========================================================================

(function () {
  'use strict';

  var track = document.querySelector('[data-social-facilities-track]');

  if (!track) {
    return;
  }

  var prevBtn = document.querySelector('[data-social-facilities-prev]');
  var nextBtn = document.querySelector('[data-social-facilities-next]');

  function scrollByCard(direction) {
    var card = track.querySelector('.social-facility-card');

    if (!card) {
      return;
    }

    var trackStyle = window.getComputedStyle(track);
    var gap = parseFloat(trackStyle.columnGap || trackStyle.gap || '0') || 0;
    var distance = (card.getBoundingClientRect().width + gap) * direction;
    var reducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    track.scrollBy({ left: distance, behavior: reducedMotion ? 'auto' : 'smooth' });
  }

  if (prevBtn) {
    prevBtn.addEventListener('click', function () {
      scrollByCard(-1);
    });
  }

  if (nextBtn) {
    nextBtn.addEventListener('click', function () {
      scrollByCard(1);
    });
  }
})();


// ==========================================================================
// Contact Form
// Progressive enhancement only — the form is a plain server-rendered POST
// and works with JS disabled. Two additions: (1) a submit-button loading
// state, guarding against double submits while the request is in flight;
// (2) moving focus to the Success Message on load so screen reader users
// hear the confirmation immediately rather than needing to find it
// (docs/05_Animations.md "Forms" — feedback must be immediate).
// ==========================================================================

(function () {
  'use strict';

  var form = document.querySelector('.contact-form');
  var submitBtn = form ? form.querySelector('[data-contact-form-submit]') : null;

  if (form && submitBtn) {
    form.addEventListener('submit', function () {
      // The browser only dispatches 'submit' once native constraint
      // validation (required/type=email/etc.) has already passed, so no
      // extra checkValidity() guard is needed here.
      submitBtn.disabled = true;
      submitBtn.textContent = 'Gönderiliyor...';
    });
  }

  var successMessage = document.querySelector('[data-success-message]');

  if (successMessage) {
    // Deferred one macrotask out (setTimeout 0) rather than focused
    // inline here — a .focus() call made synchronously while the
    // document is still settling immediately after navigation can be
    // silently dropped, so it never visibly/programmatically takes
    // hold. This reliably lands the focus screen readers need to
    // announce the confirmation (docs/05_Animations.md "Forms" —
    // feedback must be immediate). Shared by both Contact and Career —
    // whichever page rendered [data-success-message], if any.
    window.setTimeout(function () {
      successMessage.focus();
    }, 0);
  }
})();


// ==========================================================================
// Career Form
// Same progressive-enhancement submit-button loading state as the Contact
// Form module above (a plain server-rendered POST, works with JS
// disabled) — kept as its own block rather than widening that module's
// selector, since a CV upload's "Gönderiliyor..." label reads slightly
// differently and the two forms may one day diverge further.
// ==========================================================================

(function () {
  'use strict';

  var form = document.querySelector('.career-form');
  var submitBtn = form ? form.querySelector('[data-career-form-submit]') : null;

  if (form && submitBtn) {
    form.addEventListener('submit', function () {
      submitBtn.disabled = true;
      submitBtn.textContent = 'Gönderiliyor...';
    });
  }
})();


// ==========================================================================
// Global Search Overlay
// Global Navigation & Search milestone (docs/14_Decisions.md). Opened by
// the Navbar's #search-trigger; fetches the Search Service's index once
// from GET /api/search/index (cached in `index` below) and filters it
// client-side on every keystroke — no per-keystroke request, no external
// search library. Follows the WAI-ARIA combobox/listbox pattern: focus
// never leaves the input, ArrowUp/ArrowDown move an .is-active option and
// update aria-activedescendant, Enter navigates to it. Open/close and its
// Tab focus trap follow the same shape as the Video Showcase / Media
// Viewer modals above, generalised to a two-stop trap (input, close
// button) since results aren't real tab stops in this pattern.
//
// Search Engine Completion milestone (docs/14_Decisions.md): matching is
// Turkish-diacritic-insensitive (İnşaat/insaat/İNŞAAT/inşaat all fold to
// the same string) and multi-word ("AND" across words, each word matched
// as a partial/substring hit against title+description+category+the
// server's non-rendered Keywords field). Every char in TR_FOLD_MAP maps to
// exactly one output char, so a normalized string stays index-aligned
// with its source — that alignment is what lets highlight() re-wrap the
// *original* (correctly-cased, un-folded) text in <mark> using offsets
// found in the normalized haystack.
// ==========================================================================

(function () {
  'use strict';

  var trigger = document.getElementById('search-trigger');
  var overlay = document.querySelector('[data-search-overlay]');

  if (!trigger || !overlay) {
    return;
  }

  var input = overlay.querySelector('[data-search-input]');
  var resultsList = overlay.querySelector('[data-search-results]');
  var emptyState = overlay.querySelector('[data-search-empty]');
  var emptyQueryEl = overlay.querySelector('[data-search-empty-query]');
  var closeBtn = overlay.querySelector('.search-overlay-close');
  var closers = Array.prototype.slice.call(overlay.querySelectorAll('[data-search-close]'));

  var index = null;
  var indexPromise = null;
  var activeIndex = -1;
  var isOpen = false;
  var lastFocused = null;

  var TR_FOLD_MAP = {
    'İ': 'i', 'I': 'i', 'ı': 'i',
    'Ş': 's', 'ş': 's',
    'Ğ': 'g', 'ğ': 'g',
    'Ü': 'u', 'ü': 'u',
    'Ö': 'o', 'ö': 'o',
    'Ç': 'c', 'ç': 'c'
  };

  var HTML_ESCAPE_MAP = {
    '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;'
  };

  // Case- and Turkish-diacritic-folding, one output char per input char —
  // deliberately not String.toLocaleLowerCase('tr'), which turns 'İ' into
  // a two-code-unit "i" + combining-dot sequence and would break the
  // index alignment highlight() depends on.
  function foldChar(ch) {
    return TR_FOLD_MAP[ch] || ch.toLowerCase();
  }

  function normalize(str) {
    var out = '';
    var s = str || '';
    for (var i = 0; i < s.length; i++) {
      out += foldChar(s[i]);
    }
    return out;
  }

  function queryWords(query) {
    return normalize(query).split(/\s+/).filter(Boolean);
  }

  function escapeHtml(str) {
    return str.replace(/[&<>"']/g, function (ch) {
      return HTML_ESCAPE_MAP[ch];
    });
  }

  // Wraps every occurrence of every query word in <mark>, escaping the
  // surrounding text first — the only unescaped markup ever introduced is
  // the literal <mark>/</mark> this function writes itself, so server
  // data (title/description) can never inject HTML.
  function highlight(text, words) {
    var safeText = text || '';
    if (!words.length) {
      return escapeHtml(safeText);
    }

    var normalized = normalize(safeText);
    var ranges = [];

    words.forEach(function (word) {
      var from = 0;
      var found;
      while ((found = normalized.indexOf(word, from)) !== -1) {
        ranges.push([found, found + word.length]);
        from = found + word.length;
      }
    });

    if (!ranges.length) {
      return escapeHtml(safeText);
    }

    ranges.sort(function (a, b) {
      return a[0] - b[0];
    });

    var merged = [ranges[0]];
    for (var i = 1; i < ranges.length; i++) {
      var last = merged[merged.length - 1];
      if (ranges[i][0] <= last[1]) {
        last[1] = Math.max(last[1], ranges[i][1]);
      } else {
        merged.push(ranges[i]);
      }
    }

    var html = '';
    var cursor = 0;
    merged.forEach(function (range) {
      html += escapeHtml(safeText.slice(cursor, range[0]));
      html += '<mark>' + escapeHtml(safeText.slice(range[0], range[1])) + '</mark>';
      cursor = range[1];
    });
    html += escapeHtml(safeText.slice(cursor));

    return html;
  }

  function loadIndex() {
    if (indexPromise) {
      return indexPromise;
    }

    indexPromise = fetch('/api/search/index')
      .then(function (response) {
        return response.ok ? response.json() : [];
      })
      .catch(function () {
        return [];
      })
      .then(function (data) {
        // Precomputed once per item here rather than per keystroke in
        // filter() — the index only loads once per page view, so this
        // keeps every subsequent keystroke's cost to a single indexOf
        // scan per item instead of re-normalizing four fields each time.
        data.forEach(function (item) {
          item._haystack = normalize([item.title, item.description, item.category, item.keywords]
            .filter(Boolean)
            .join(' '));
        });
        index = data;
        return data;
      });

    return indexPromise;
  }

  function updateActiveDescendant() {
    if (activeIndex === -1) {
      input.removeAttribute('aria-activedescendant');
      return;
    }
    input.setAttribute('aria-activedescendant', 'search-result-' + activeIndex);
  }

  // Empty query state: neither the results list nor the "no results"
  // empty-state message is shown — only the search field itself, until
  // the visitor types the first character.
  function renderNone() {
    resultsList.innerHTML = '';
    resultsList.hidden = true;
    emptyState.hidden = true;
    activeIndex = -1;
    updateActiveDescendant();
  }

  function render(items, words) {
    resultsList.innerHTML = '';
    activeIndex = items.length ? 0 : -1;

    items.forEach(function (item, i) {
      var li = document.createElement('li');
      li.className = 'search-result' + (i === 0 ? ' is-active' : '');
      li.setAttribute('role', 'option');
      li.id = 'search-result-' + i;

      var a = document.createElement('a');
      a.className = 'search-result-link';
      a.href = item.url;
      a.tabIndex = -1;

      var categoryEl = document.createElement('span');
      categoryEl.className = 'search-result-category';
      categoryEl.textContent = item.category;

      var titleEl = document.createElement('span');
      titleEl.className = 'search-result-title';
      titleEl.innerHTML = highlight(item.title, words || []);

      var descEl = document.createElement('span');
      descEl.className = 'search-result-desc';
      descEl.innerHTML = highlight(item.description, words || []);

      a.appendChild(categoryEl);
      a.appendChild(titleEl);
      a.appendChild(descEl);
      li.appendChild(a);
      resultsList.appendChild(li);
    });

    resultsList.hidden = items.length === 0;
    emptyState.hidden = items.length !== 0;
    if (items.length === 0 && emptyQueryEl) {
      emptyQueryEl.textContent = input.value.trim();
    }

    updateActiveDescendant();
  }

  function setActive(nextIndex) {
    var items = Array.prototype.slice.call(resultsList.querySelectorAll('.search-result'));
    if (!items.length) {
      return;
    }

    items.forEach(function (el) {
      el.classList.remove('is-active');
    });

    activeIndex = ((nextIndex % items.length) + items.length) % items.length;
    items[activeIndex].classList.add('is-active');
    items[activeIndex].scrollIntoView({ block: 'nearest' });
    updateActiveDescendant();
  }

  function filter(query) {
    if (!index) {
      return;
    }

    var words = queryWords(query);

    if (!words.length) {
      renderNone();
      return;
    }

    // AND across words: every word must appear somewhere in the item's
    // haystack, each as a partial/substring match — so "ançın vadi"
    // narrows to results containing both, in any field, any order.
    var matches = index.filter(function (item) {
      return words.every(function (word) {
        return item._haystack.indexOf(word) !== -1;
      });
    });

    render(matches, words);
  }

  function focusableElements() {
    return [input, closeBtn].filter(Boolean);
  }

  function onKeydown(event) {
    if (event.key === 'Escape') {
      close();
      return;
    }

    if (event.key === 'ArrowDown') {
      event.preventDefault();
      setActive(activeIndex + 1);
      return;
    }

    if (event.key === 'ArrowUp') {
      event.preventDefault();
      setActive(activeIndex - 1);
      return;
    }

    if (event.key === 'Enter') {
      var activeLink = resultsList.querySelector('.search-result.is-active .search-result-link');
      if (activeLink) {
        event.preventDefault();
        window.location.href = activeLink.href;
      }
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

  function open() {
    if (isOpen) {
      return;
    }

    isOpen = true;
    lastFocused = document.activeElement;
    trigger.setAttribute('aria-expanded', 'true');
    input.setAttribute('aria-expanded', 'true');
    document.body.classList.add('no-scroll');
    document.addEventListener('keydown', onKeydown);

    overlay.classList.add('is-open');
    input.focus();

    loadIndex().then(function () {
      // A closed-before-loaded race would otherwise render into a
      // dialog the visitor already dismissed. The input is always empty
      // at this point (open() only fires right after a fresh trigger
      // click or close()'s reset), so there's nothing to filter yet —
      // just make sure no stale results/empty-state linger from a
      // previous session.
      if (isOpen) {
        renderNone();
      }
    });
  }

  function close() {
    if (!isOpen) {
      return;
    }

    isOpen = false;
    overlay.classList.remove('is-open');
    trigger.setAttribute('aria-expanded', 'false');
    input.setAttribute('aria-expanded', 'false');
    document.body.classList.remove('no-scroll');
    document.removeEventListener('keydown', onKeydown);
    input.value = '';

    if (lastFocused && typeof lastFocused.focus === 'function') {
      lastFocused.focus();
    } else {
      trigger.focus();
    }
  }

  trigger.addEventListener('click', function () {
    if (isOpen) {
      close();
    } else {
      open();
    }
  });

  closers.forEach(function (closer) {
    closer.addEventListener('click', close);
  });

  input.addEventListener('input', function () {
    filter(input.value);
  });

  resultsList.addEventListener('click', function (event) {
    var link = event.target.closest ? event.target.closest('.search-result-link') : null;
    if (link) {
      // Native navigation already follows the anchor's href — this only
      // needs to release the scroll lock/listeners before the browser
      // unloads the page.
      close();
    }
  });
})();


// ==========================================================================
// Navbar — Transparent Over Hero
// Nysa Gold Residence reference redesign (2026-08-07), rolled out to every
// Project Detail page (2026-08-08) and the Home Hero (2026-08-10). Scoped
// by .hero--overlap-navbar's presence, not by page or slug — any Hero that
// renders that class (HeroBannerProjectDetail/Default.cshtml,
// HeroBanner/Default.cshtml) gets this behavior automatically, so this is a
// safe no-op on Projects/About/etc., whose Hero never carries the class.
// Toggles .navbar--transparent (site.css) while the Hero is in view; CSS
// handles the smooth revert transition once the observer reports the Hero
// has scrolled past.
//
// UI revision (2026-08-07): also swaps the navbar logo's src between its
// two data-logo-src-* attributes (Navbar/Default.cshtml) in lockstep with
// the same transparent/solid state — no second navbar, no server branching,
// just the one existing observer driving both.
// ==========================================================================

(function () {
  'use strict';

  var navbar = document.querySelector('.navbar');
  var hero = document.querySelector('.hero--overlap-navbar');
  var logo = document.querySelector('[data-navbar-logo]');

  if (!navbar || !hero || !('IntersectionObserver' in window)) {
    return;
  }

  var solidLogoSrc = logo ? logo.getAttribute('data-logo-src-solid') : null;
  var transparentLogoSrc = logo ? logo.getAttribute('data-logo-src-transparent') : null;

  var observer = new IntersectionObserver(function (entries) {
    entries.forEach(function (entry) {
      navbar.classList.toggle('navbar--transparent', entry.isIntersecting);

      if (logo && solidLogoSrc && transparentLogoSrc) {
        logo.src = entry.isIntersecting ? transparentLogoSrc : solidLogoSrc;
      }
    });
  }, { threshold: 0, rootMargin: '-1px 0px 0px 0px' });

  observer.observe(hero);
})();


// ==========================================================================
// Project Concept — Featured Media Carousel
// Nysa Gold Residence reference redesign (2026-08-07). Pages the Concept
// section's media card through the project's own "Exterior" gallery images
// (_ProjectConcept.cshtml's data-concept-images island — no new entities,
// same data the Gallery section already renders) via the Prev/Next arrows
// beneath the floating panel's description. Also keeps the play button's
// Media Viewer trigger attributes in sync with whichever image is showing.
// Gated by [data-concept-images]'s presence (only rendered when a project
// has 2+ Exterior images), so this is a safe no-op on every other project's
// page today.
// ==========================================================================

(function () {
  'use strict';

  var dataIsland = document.querySelector('[data-concept-images]');
  var imageEl = document.querySelector('[data-concept-image]');
  var playTrigger = document.querySelector('[data-concept-play]');
  var prevBtn = document.querySelector('[data-concept-prev]');
  var nextBtn = document.querySelector('[data-concept-next]');

  if (!dataIsland || !imageEl || !playTrigger || !prevBtn || !nextBtn) {
    return;
  }

  var items = Array.prototype.slice.call(dataIsland.querySelectorAll('[data-concept-image-item]'));

  if (items.length < 2) {
    return;
  }

  var index = 0;

  function render() {
    var item = items[index];
    var src = item.getAttribute('data-src');
    var alt = item.getAttribute('data-alt') || '';

    imageEl.src = src;
    imageEl.alt = alt;
    playTrigger.setAttribute('data-media-viewer-src', src);
    playTrigger.setAttribute('data-media-viewer-alt', alt);
  }

  prevBtn.addEventListener('click', function () {
    index = (index - 1 + items.length) % items.length;
    render();
  });

  nextBtn.addEventListener('click', function () {
    index = (index + 1) % items.length;
    render();
  });
})();

// ==========================================================================
// Project Concept — Video Play/Revert
// Davutlar D Latis Media phase (2026-08-09). Only rendered by
// _ProjectConcept.cshtml when a project has both a concept video and a
// poster — a safe no-op on every other project's page today. Clicking Play
// hides the poster, shows the preload="none" <video>, and starts playback;
// when the video ends (or is paused back to the start), the poster and Play
// button return — no Media Viewer lightbox involved.
// ==========================================================================

(function () {
  'use strict';

  var card = document.querySelector('[data-concept-video-card]');
  var poster = document.querySelector('[data-concept-video-poster]');
  var video = document.querySelector('[data-concept-video]');
  var playBtn = document.querySelector('[data-concept-video-play]');

  if (!card || !poster || !video || !playBtn) {
    return;
  }

  function showPoster() {
    video.hidden = true;
    poster.hidden = false;
    playBtn.hidden = false;
    video.pause();
    video.currentTime = 0;
  }

  function playVideo() {
    poster.hidden = true;
    playBtn.hidden = true;
    video.hidden = false;
    video.play();
  }

  playBtn.addEventListener('click', playVideo);
  video.addEventListener('ended', showPoster);
})();


// ==========================================================================
// Project Concept — Video Carousel (Davutlar D Latis Concept redesign,
// 2026-08-09)
// Slides .concept-carousel-track and .concept-carousel-panel-track together
// via transform: translateX so the visible poster/video always stays
// paired with its own heading and paragraph
// (_ProjectConcept.cshtml's [data-concept-carousel-slide]/
// [data-concept-carousel-panel-slide], one pair per ProjectConceptVideo
// row). Prev/Next wrap around, matching every other carousel on the page.
// Only rendered for projects with 2+ concept videos — a safe no-op
// everywhere else since [data-concept-carousel] won't exist.
// ==========================================================================

(function () {
  'use strict';

  var root = document.querySelector('[data-concept-carousel]');
  if (!root) {
    return;
  }

  var mediaTrack = root.querySelector('[data-concept-carousel-track]');
  var panelTrack = root.querySelector('[data-concept-carousel-panel-track]');
  var slides = Array.prototype.slice.call(root.querySelectorAll('[data-concept-carousel-slide]'));
  var panelSlides = Array.prototype.slice.call(root.querySelectorAll('[data-concept-carousel-panel-slide]'));
  var prevBtn = root.querySelector('[data-concept-carousel-prev]');
  var nextBtn = root.querySelector('[data-concept-carousel-next]');

  if (!mediaTrack || !panelTrack || slides.length < 2) {
    return;
  }

  var index = 0;

  // Keeps exactly one #project-concept-heading in the DOM at all times —
  // the outer <section>'s aria-labelledby target — moving it onto whichever
  // slide's <h2> is currently active so assistive tech always announces the
  // heading that's actually visible.
  function render() {
    mediaTrack.style.transform = 'translateX(-' + (index * 100) + '%)';
    panelTrack.style.transform = 'translateX(-' + (index * 100) + '%)';

    slides.forEach(function (slide, i) {
      var active = i === index;
      slide.setAttribute('aria-hidden', active ? 'false' : 'true');

      // Video branch's trigger carries [data-concept-carousel-play]; the
      // image branch's (La Fiore Karabağ 2. Etap) is a plain
      // [data-media-viewer-trigger] — either way, it's whichever single
      // interactive element the slide contains.
      var triggerBtn = slide.querySelector('[data-concept-carousel-play], [data-media-viewer-trigger]');
      if (triggerBtn) {
        triggerBtn.tabIndex = active ? 0 : -1;
      }
    });

    panelSlides.forEach(function (panelSlide, i) {
      var active = i === index;
      panelSlide.setAttribute('aria-hidden', active ? 'false' : 'true');

      var heading = panelSlide.querySelector('[data-concept-carousel-heading]');
      if (!heading) {
        return;
      }

      if (active) {
        heading.id = 'project-concept-heading';
      } else if (heading.id === 'project-concept-heading') {
        heading.removeAttribute('id');
      }
    });
  }

  function show(nextIndex) {
    index = (nextIndex + slides.length) % slides.length;
    render();
  }

  if (prevBtn) {
    prevBtn.addEventListener('click', function () {
      show(index - 1);
    });
  }

  if (nextBtn) {
    nextBtn.addEventListener('click', function () {
      show(index + 1);
    });
  }
})();


// ==========================================================================
// Project Concept / Gallery — Video Modal (Davutlar D Latis Concept
// redesign, 2026-08-09; generalized to Gallery videos, 2026-08-09)
// Opens the shared #concept-video-modal (reuses .video-modal's shell — see
// site.css Section 19) with whichever trigger's video/poster the visitor
// clicked Play on — either a Concept carousel slide ([data-concept-
// carousel-play]) or a Gallery video card ([data-gallery-video-play]), both
// carrying the same data-video-src/data-poster-src/data-video-title
// attributes, so this one modal instance serves both features (and any
// future project's videos in either place) without a second implementation.
// Follows the same focus-trap / Escape-to-close / body-scroll-lock /
// pause-on-close shape as the Video Showcase and Media Viewer modals above
// — the one difference is the <video>'s src is set fresh on every open
// rather than baked in at render time, since one modal here can serve many
// different videos. Closing never touches the Concept carousel's own slide
// index (above) — the carousel is exactly where the visitor left it once
// the modal closes, and the page itself never scrolls/jumps since the
// modal is a fixed overlay, not a navigation.
// ==========================================================================

(function () {
  'use strict';

  var modal = document.querySelector('[data-concept-video-modal]');
  var playButtons = Array.prototype.slice.call(document.querySelectorAll('[data-concept-carousel-play], [data-gallery-video-play]'));

  if (!modal || !playButtons.length) {
    return;
  }

  var video = modal.querySelector('[data-concept-video-modal-video]');
  var closers = Array.prototype.slice.call(modal.querySelectorAll('[data-concept-video-modal-close]'));
  var lastFocused = null;

  function focusableElements() {
    return Array.prototype.slice.call(
      modal.querySelectorAll('button, video[controls]')
    ).filter(function (el) {
      return !el.hasAttribute('disabled');
    });
  }

  function onKeydown(event) {
    if (event.key === 'Escape') {
      close();
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

  function open(trigger) {
    var src = trigger.getAttribute('data-video-src');
    var poster = trigger.getAttribute('data-poster-src');
    var title = trigger.getAttribute('data-video-title');

    if (!src || !video) {
      return;
    }

    video.poster = poster || '';
    video.src = src;
    video.load();

    if (title) {
      modal.setAttribute('aria-label', title);
    }

    lastFocused = trigger;
    modal.hidden = false;
    document.body.classList.add('no-scroll');
    document.addEventListener('keydown', onKeydown);

    video.play().catch(function () {
      // Autoplay-on-open can be blocked (e.g. low-power mode) — the visitor
      // still sees the modal with native controls and can press Play
      // themselves; nothing more to do here.
    });

    var closeBtn = modal.querySelector('.video-modal-close');
    if (closeBtn) {
      closeBtn.focus();
    }
  }

  function close() {
    modal.hidden = true;
    document.body.classList.remove('no-scroll');
    document.removeEventListener('keydown', onKeydown);

    if (video) {
      video.pause();
      video.removeAttribute('src');
      video.load();
    }

    if (lastFocused && typeof lastFocused.focus === 'function') {
      lastFocused.focus();
    }
  }

  playButtons.forEach(function (button) {
    button.addEventListener('click', function () {
      open(button);
    });
  });

  closers.forEach(function (closer) {
    closer.addEventListener('click', close);
  });
})();
