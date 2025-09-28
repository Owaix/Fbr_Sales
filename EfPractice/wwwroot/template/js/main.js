(function ($) {
    "use strict";

    const safeGetCtx = (selector) => {
        const $el = $(selector);
        if ($el.length === 0) return null;
        const node = $el.get(0);
        if (!node) return null;
        if (node.tagName !== 'CANVAS') return null;
        return node.getContext ? node.getContext('2d') : null;
    };

    const safeChart = (selector, cfgFactory) => {
        if (typeof Chart === 'undefined') return; // Chart.js not loaded
        const ctx = safeGetCtx(selector);
        if (!ctx) return; // canvas absent on this page
        try {
            new Chart(ctx, cfgFactory());
        } catch (e) { console.warn('Chart init failed for', selector, e); }
    };

    // Spinner
    var spinner = function () {
        setTimeout(function () {
            if ($('#spinner').length > 0) {
                $('#spinner').removeClass('show');
            }
        }, 1);
    };
    spinner();

    // Back to top button
    $(window).on('scroll', function () {
        if ($(this).scrollTop() > 300) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').on('click', function () {
        $('html, body').animate({ scrollTop: 0 }, 1500, 'easeInOutExpo');
        return false;
    });

    // Sidebar Toggler
    $('.sidebar-toggler').on('click', function () {
        $('.sidebar, .content').toggleClass("open");
        return false;
    });

    // Progress Bar
    if ($('.pg-bar').length && $.fn.waypoint) {
        $('.pg-bar').waypoint(function () {
            $('.progress .progress-bar').each(function () {
                $(this).css("width", $(this).attr("aria-valuenow") + '%');
            });
        }, { offset: '80%' });
    }

    // Calendar (guard plugin existence)
    if ($('#calender').length && $.fn.datetimepicker) {
        $('#calender').datetimepicker({ inline: true, format: 'L' });
    }

    // Testimonials carousel
    if ($('.testimonial-carousel').length && $.fn.owlCarousel) {
        $(".testimonial-carousel").owlCarousel({
            autoplay: true,
            smartSpeed: 1000,
            items: 1,
            dots: true,
            loop: true,
            nav: false
        });
    }

    // Charts (only create if canvas exists)
    safeChart('#worldwide-sales', () => ({
        type: 'bar',
        data: {
            labels: ["2016", "2017", "2018", "2019", "2020", "2021", "2022"],
            datasets: [
                { label: "USA", data: [15, 30, 55, 65, 60, 80, 95], backgroundColor: "rgba(0, 156, 255, .7)" },
                { label: "UK", data: [8, 35, 40, 60, 70, 55, 75], backgroundColor: "rgba(0, 156, 255, .5)" },
                { label: "AU", data: [12, 25, 45, 55, 65, 70, 60], backgroundColor: "rgba(0, 156, 255, .3)" }
            ]
        },
        options: { responsive: true }
    }));

    safeChart('#salse-revenue', () => ({
        type: 'line',
        data: {
            labels: ["2016", "2017", "2018", "2019", "2020", "2021", "2022"],
            datasets: [
                { label: "Salse", data: [15, 30, 55, 45, 70, 65, 85], backgroundColor: "rgba(0, 156, 255, .5)", fill: true },
                { label: "Revenue", data: [99, 135, 170, 130, 190, 180, 270], backgroundColor: "rgba(0, 156, 255, .3)", fill: true }
            ]
        },
        options: { responsive: true }
    }));

    safeChart('#line-chart', () => ({
        type: 'line',
        data: {
            labels: [50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150],
            datasets: [{ label: 'Salse', fill: false, backgroundColor: 'rgba(0, 156, 255, .3)', data: [7, 8, 8, 9, 9, 9, 10, 11, 14, 14, 15] }]
        },
        options: { responsive: true }
    }));

    safeChart('#bar-chart', () => ({
        type: 'bar',
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: { responsive: true }
    }));

    safeChart('#pie-chart', () => ({
        type: 'pie',
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: { responsive: true }
    }));

    safeChart('#doughnut-chart', () => ({
        type: 'doughnut',
        data: {
            labels: ["Italy", "France", "Spain", "USA", "Argentina"],
            datasets: [{
                backgroundColor: [
                    "rgba(0, 156, 255, .7)",
                    "rgba(0, 156, 255, .6)",
                    "rgba(0, 156, 255, .5)",
                    "rgba(0, 156, 255, .4)",
                    "rgba(0, 156, 255, .3)"
                ],
                data: [55, 49, 44, 24, 15]
            }]
        },
        options: { responsive: true }
    }));

})(jQuery);

