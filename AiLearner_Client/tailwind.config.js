const { addDynamicIconSelectors } = require("@iconify/tailwind");

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
      textShadow: ["responsive"],
    },
  },
  plugins: [addDynamicIconSelectors()],
};
