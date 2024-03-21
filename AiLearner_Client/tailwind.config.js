/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
    theme: {
    extend: {
      fontFamily:{
        'zabal': ['Zabal', 'sans-serif']
      },
      screens: {
        'max-400': {'max': '399px'},
      },
      textShadow: ['responsive'],
    },
  },
  plugins: [],
}

