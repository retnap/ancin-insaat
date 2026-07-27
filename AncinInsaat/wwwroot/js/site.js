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
