/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
    theme: {
    extend: {
      screens: {
        'max-400': {'max': '399px'},
      },
    },
  },
  plugins: [],
}

