const { addDynamicIconSelectors } = require("@iconify/tailwind");
const { default: shadows } = require("@mui/material/styles/shadows");

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{html,ts}"],
  theme: {
    extend: {
      width: {
        inherit: "inherit",
      },
      fontFamily: {
        zabal: ["Zabal", "sans-serif"],
      },
      screens: {
        "max-400": { max: "399px" },
      },
      textShadow: {
        sm: "-2px 2px 2px rgba(0, 0, 0, 0.5)",
        DEFAULT: "2px 2px 2px rgba(0, 0, 0, 0.5)",
        white: " -2px 2px 10px rgba(255,255,255,0.6)",
        lg: "0 8px 16px var(--tw-shadow-color)",
      },
      animation: {
        "scale-up": "scaleUp 1.5s ease-out forwards",
        "right-load": "rightLoad 0.8s ease-out forwards",
        "right-bounce": "bounceOnce 2s ease forwards",
      },
      dropShadow: {
        'img-shadow': '10px 10px 10px rgba(0, 0, 0, 0.6)'
      }
    },
  },
  plugins: [
    addDynamicIconSelectors(),
    function ({ matchUtilities, theme }) {
      matchUtilities(
        {
          "text-shadow": (value) => ({
            textShadow: value,
          }),
        },
        { values: theme("textShadow") }
      );
    },
  ],
};
