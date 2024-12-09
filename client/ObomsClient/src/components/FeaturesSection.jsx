import { useRef, useState, useEffect } from 'react';
import { motion, useScroll, useTransform } from 'framer-motion';
import PropTypes from 'prop-types';
import { 
  UserGroupIcon, 
  CalendarIcon, 
  Cog6ToothIcon,
  ChartBarIcon,
  ShieldCheckIcon,
  BellAlertIcon
} from '@heroicons/react/24/outline';
import OptimizedImage from './OptimizedImage';

const features = [
  {
    icon: UserGroupIcon,
    title: "Social Integration",
    description: "Connect with friends, share experiences, and build your network within our vibrant community. Discover new venues through trusted recommendations.",
    image: {
      src: "https://images.unsplash.com/photo-1543007630-9710e4a00a20?w=800&auto=format&fit=crop&q=60",
      alt: "People enjoying social dining experience",
      width: 800,
      height: 600
    },
    metrics: [
      { label: "Active Users", value: "10K+" },
      { label: "Partner Venues", value: "500+" }
    ]
  },
  {
    icon: CalendarIcon,
    title: "Seamless Booking",
    description: "Book tables instantly with real-time availability across multiple venues. Our smart scheduling system ensures you always get the perfect spot.",
    image: {
      src: "https://www.stayntouch.com/wp-content/uploads/2024/05/seamless-booking-engine-opt.png",
      alt: "Modern booking interface on mobile",
      width: 800,
      height: 600
    },
    metrics: [
      { label: "Instant Bookings", value: "1M+" },
      { label: "Success Rate", value: "99.9%" }
    ]
  },
  {
    icon: Cog6ToothIcon,
    title: "Smart Management",
    description: "Powerful tools for businesses to optimize operations, track performance, and grow revenue. Get insights into customer preferences and streamline your service.",
    image: {
      src: "https://images.unsplash.com/photo-1460925895917-afdab827c52f?w=800&auto=format&fit=crop&q=60",
      alt: "Business analytics dashboard",
      width: 800,
      height: 600
    },
    metrics: [
      { label: "Time Saved", value: "20hrs/week" },
      { label: "Customer Satisfaction", value: "4.9/5" }
    ]
  },
  {
    icon: ChartBarIcon,
    title: "Advanced Analytics",
    description: "Make data-driven decisions with our powerful analytics suite. Track customer behavior, predict busy periods, and optimize your pricing strategy.",
    image: {
      src: "https://images.unsplash.com/photo-1551288049-bebda4e38f71?w=800&auto=format&fit=crop&q=60",
      alt: "Advanced analytics dashboard",
      width: 800,
      height: 600
    },
    metrics: [
      { label: "Data Points", value: "1B+" },
      { label: "Accuracy Rate", value: "98%" }
    ]
  },
  {
    icon: ShieldCheckIcon,
    title: "Enterprise Security",
    description: "Enterprise-grade security with 99.9% uptime. Your data is encrypted end-to-end, and our system is compliant with global security standards.",
    image: {
      src: "https://images.unsplash.com/photo-1563986768609-322da13575f3?w=800&auto=format&fit=crop&q=60",
      alt: "Security dashboard interface",
      width: 800,
      height: 600
    },
    metrics: [
      { label: "Uptime", value: "99.9%" },
      { label: "Security Score", value: "A+" }
    ]
  },
  {
    icon: BellAlertIcon,
    title: "Smart Notifications",
    description: "Stay informed with intelligent notifications. Get real-time updates about bookings, customer feedback, and important business metrics.",
    image: {
      src: "https://images.unsplash.com/photo-1512428559087-560fa5ceab42?w=800&auto=format&fit=crop&q=60",
      alt: "Mobile notifications interface",
      width: 800,
      height: 600
    },
    metrics: [
      { label: "Delivery Rate", value: "99.8%" },
      { label: "Response Time", value: "<1min" }
    ]
  }
];

const FeatureMetric = ({ label, value }) => (
  <div className="bg-[#1C1C35]/50 rounded-lg px-4 py-3 border border-blue-900/20">
    <div 
      className="text-blue-400 font-semibold text-lg"
      aria-label={`${label}: ${value}`}
    >
      {value}
    </div>
    <div className="text-gray-400 text-sm font-medium">
      {label}
    </div>
  </div>
);

FeatureMetric.propTypes = {
  label: PropTypes.string.isRequired,
  value: PropTypes.string.isRequired
};

const FeatureCard = ({ feature, isActive }) => (
  <div 
    className={`bg-[#252543] rounded-2xl p-6 sm:p-8 shadow-xl transition-all duration-300 ${
      isActive ? 'opacity-100 translate-y-0' : 'opacity-40 translate-y-8'
    }`}
  >
    <div className="flex items-start space-x-4 mb-6">
      <div className="bg-blue-500/10 rounded-xl p-2.5 sm:p-3">
        <feature.icon className="w-5 h-5 sm:w-6 sm:h-6 text-blue-400" />
      </div>
      <h3 className="text-xl sm:text-2xl font-bold text-white">
        {feature.title}
      </h3>
    </div>

    <p className="text-sm sm:text-base text-gray-400 leading-relaxed mb-6 sm:mb-8">
      {feature.description}
    </p>

    <div className="space-y-3 sm:space-y-4">
      <h4 className="text-xs sm:text-sm font-semibold text-gray-400 uppercase tracking-wider">
        Key Metrics
      </h4>
      <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 sm:gap-4">
        {feature.metrics.map((metric) => (
          <div 
            key={metric.label}
            className="bg-[#1C1C35]/50 rounded-lg px-3 sm:px-4 py-2.5 sm:py-3 border border-blue-900/20"
          >
            <div className="text-blue-400 font-semibold text-base sm:text-lg">
              {metric.value}
            </div>
            <div className="text-gray-400 text-xs sm:text-sm">
              {metric.label}
            </div>
          </div>
        ))}
      </div>
    </div>
  </div>
);

FeatureCard.propTypes = {
  feature: PropTypes.shape({
    icon: PropTypes.elementType.isRequired,
    title: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    metrics: PropTypes.arrayOf(PropTypes.shape({
      label: PropTypes.string.isRequired,
      value: PropTypes.string.isRequired
    })).isRequired
  }).isRequired,
  isActive: PropTypes.bool.isRequired
};

const SideNav = ({ activeIndex, features, onDotClick }) => (
  <div className="fixed left-8 top-1/2 -translate-y-1/2 z-30">
    <div className="flex flex-col items-center space-y-3">
      {features.map((feature, index) => (
        <button
          key={feature.title}
          onClick={() => onDotClick(index)}
          className="group relative flex items-center"
          aria-label={`Navigate to ${feature.title} section`}
        >
          <div 
            className={`w-2.5 h-2.5 rounded-full transition-all duration-200 ${
              activeIndex === index 
                ? 'bg-blue-400 scale-110' 
                : 'bg-gray-600 hover:bg-gray-400'
            }`}
          />
          <div className="absolute left-5 px-2 py-1 rounded-md bg-[#252543]/90 backdrop-blur-sm opacity-0 group-hover:opacity-100 transition-opacity duration-200 whitespace-nowrap">
            <span className="text-xs text-white font-medium">{feature.title}</span>
          </div>
        </button>
      ))}
    </div>
  </div>
);

SideNav.propTypes = {
  activeIndex: PropTypes.number.isRequired,
  features: PropTypes.arrayOf(PropTypes.shape({
    title: PropTypes.string.isRequired
  })).isRequired,
  onDotClick: PropTypes.func.isRequired
};

export default function FeaturesSection() {
  const containerRef = useRef(null);
  const [activeIndex, setActiveIndex] = useState(0);
  const [showNav, setShowNav] = useState(false);
  const { scrollYProgress } = useScroll({
    target: containerRef,
    offset: ["start start", "end end"]
  });

  // Even faster header opacity transition
  const headerOpacity = useTransform(scrollYProgress, [0, 0.02], [0, 1]);

  // Scroll to section with offset positioning
  const handleDotClick = (index) => {
    const sections = document.querySelectorAll('[data-feature-section]');
    const section = sections[index];
    
    if (section) {
      const viewportHeight = window.innerHeight;
      const sectionRect = section.getBoundingClientRect();
      const currentScrollY = window.scrollY;
      
      // Calculate the target scroll position to center the section and move up 90px
      const targetScrollY = currentScrollY + sectionRect.top - (viewportHeight - sectionRect.height) / 2 - 90;
      
      window.scrollTo({
        top: targetScrollY,
        behavior: 'smooth'
      });
      
      setActiveIndex(index);
    }
  };

  // Update active section and nav visibility based on scroll position
  useEffect(() => {
    const sections = document.querySelectorAll('[data-feature-section]');
    const ctaSection = document.querySelector('[data-cta-section]');
    let timeoutId;
    
    const handleScroll = () => {
      if (timeoutId) {
        window.cancelAnimationFrame(timeoutId);
      }

      timeoutId = window.requestAnimationFrame(() => {
        const viewportHeight = window.innerHeight;
        const viewportCenter = viewportHeight / 2;
        
        // Check first feature section for showing nav
        const firstSection = sections[0];
        if (firstSection) {
          const rect = firstSection.getBoundingClientRect();
          const sectionCenter = rect.top + (rect.height / 2);
          
          // Show nav when first section reaches the target position
          if (sectionCenter <= viewportCenter) {
            // Check CTA section to hide nav
            if (ctaSection) {
              const ctaRect = ctaSection.getBoundingClientRect();
              const ctaCenter = ctaRect.top + (ctaRect.height / 2);
              
              // Hide nav when reaching middle of CTA
              setShowNav(ctaCenter > viewportCenter);
            } else {
              setShowNav(true);
            }
          } else {
            setShowNav(false);
          }
        }

        // Update active section
        sections.forEach((section, index) => {
          const rect = section.getBoundingClientRect();
          const sectionCenter = rect.top + (rect.height / 2);
          const targetPosition = viewportCenter - 90;
          
          if (Math.abs(sectionCenter - targetPosition) < rect.height / 3) {
            setActiveIndex(index);
          }
        });
      });
    };

    window.addEventListener('scroll', handleScroll);
    handleScroll(); // Initial check
    
    return () => {
      window.removeEventListener('scroll', handleScroll);
      if (timeoutId) {
        window.cancelAnimationFrame(timeoutId);
      }
    };
  }, []);

  return (
    <section 
      ref={containerRef} 
      className="relative bg-[#1C1C35] pt-8 sm:pt-12"
      aria-labelledby="features-heading"
    >
      {/* Side Navigation - Hide on mobile */}
      <motion.div 
        className="hidden md:block fixed left-4 lg:left-8 top-1/2 -translate-y-1/2 z-30"
        initial={{ opacity: 0 }}
        animate={{ opacity: showNav ? 1 : 0, pointerEvents: showNav ? 'auto' : 'none' }}
        transition={{ duration: 0.1 }}
      >
        <div className="flex flex-col items-center space-y-3">
          {features.map((feature, index) => (
            <button
              key={feature.title}
              onClick={() => handleDotClick(index)}
              className="group relative flex items-center"
              aria-label={`Navigate to ${feature.title} section`}
            >
              <div 
                className={`w-2 h-2 sm:w-2.5 sm:h-2.5 rounded-full transition-all duration-200 ${
                  activeIndex === index 
                    ? 'bg-blue-400 scale-110' 
                    : 'bg-gray-600 hover:bg-gray-400'
                }`}
              />
              <div className="absolute left-5 px-2 py-1 rounded-md bg-[#252543]/90 backdrop-blur-sm opacity-0 group-hover:opacity-100 transition-opacity duration-200 whitespace-nowrap">
                <span className="text-xs text-white font-medium">{feature.title}</span>
              </div>
            </button>
          ))}
        </div>
      </motion.div>

      {/* Sticky Header */}
      <motion.div 
        className="sticky top-16 z-20 bg-[#1C1C35]/90 backdrop-blur-sm py-4 sm:py-6 mb-6 sm:mb-8"
        style={{ opacity: headerOpacity }}
        transition={{ duration: 0.1 }}
      >
        <div className="max-w-3xl mx-auto px-4">
          <span className="block text-center text-blue-400 font-semibold text-xs uppercase tracking-wider mb-2">
            Powerful Features
          </span>
          <h2 
            id="features-heading"
            className="text-2xl sm:text-3xl md:text-4xl font-bold text-center text-white leading-tight"
          >
            Why Choose{" "}
            <span className="bg-gradient-to-r from-blue-400 to-violet-500 text-transparent bg-clip-text">
              OBOMS
            </span>
            ?
          </h2>
          <p className="text-sm sm:text-base text-gray-400 text-center mt-2 max-w-2xl mx-auto">
            Discover how our comprehensive suite of features can transform your business
          </p>
        </div>
      </motion.div>

      {/* Features */}
      <div className="relative z-10">
        {features.map((feature, index) => (
          <div
            key={feature.title}
            data-feature-section
            className="min-h-screen flex items-center"
          >
            <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 w-full">
              <div className="grid lg:grid-cols-2 gap-8 sm:gap-12 lg:gap-16 items-center">
                <FeatureCard 
                  feature={feature} 
                  isActive={index === activeIndex} 
                />
                
                <div className="relative mt-8 lg:mt-0">
                  <div className="absolute -inset-4 bg-gradient-to-r from-blue-500/10 to-violet-500/10 rounded-2xl blur-2xl" />
                  <div className="relative h-[300px] sm:h-[400px] md:h-[500px] lg:h-[600px] rounded-2xl overflow-hidden ring-1 ring-white/10">
                    <OptimizedImage
                      {...feature.image}
                      className="w-full h-full object-cover"
                      effect="fade"
                      priority={index === 0}
                    />
                    <div 
                      className="absolute inset-0 bg-gradient-to-t from-[#1C1C35] via-[#1C1C35]/50 to-transparent"
                      aria-hidden="true"
                    />
                  </div>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>

      <div 
        className="absolute inset-0 bg-gradient-to-b from-[#1C1C35] via-[#1C1C35]/95 to-[#1C1C35] pointer-events-none"
        aria-hidden="true"
      />
    </section>
  );
} 