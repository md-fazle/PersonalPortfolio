document.addEventListener("DOMContentLoaded", function() {
    const cards = document.querySelectorAll('.carousel-item .card');

    // Function to move the cards based on mouse position
    document.addEventListener('mousemove', function(event) {
        const mouseX = event.clientX;
        const mouseY = event.clientY;
        
        // Get the center of the viewport
        const centerX = window.innerWidth / 2;
        const centerY = window.innerHeight / 2;
        
        // Calculate the movement intensity based on mouse position relative to the center
        const offsetX = (mouseX - centerX) / centerX; // Horizontal movement factor
        const offsetY = (mouseY - centerY) / centerY; // Vertical movement factor
        
        // Move the cards based on mouse movement
        cards.forEach(card => {
            card.style.transform = `translateX(${offsetX * 30}px) translateY(${offsetY * 20}px)`;
        });
    });

    // Set up IntersectionObserver for card animation
    const observerOptions = {
        root: null, // observe relative to the viewport
        rootMargin: "0px",
        threshold: 0.2 // Trigger animation when 20% of the card is visible
    };

    const observer = new IntersectionObserver((entries, observer) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-card');
                observer.unobserve(entry.target); // Stop observing once it's animated
            }
        });
    }, observerOptions);

    // Observe each card for intersection
    cards.forEach(card => {
        observer.observe(card); // Observe each card for intersection
    });
});
