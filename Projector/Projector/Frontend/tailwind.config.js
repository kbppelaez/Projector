const defaultTheme = require('tailwindcss/defaultTheme')
const colors = require('tailwindcss/colors')

/** @type {import('tailwindcss').Config} */
module.exports = {
    darkMode: ["class"],
    content: [
        './Views/**/*.cshtml',
    ],
    theme: {
        container: {
            center: true,
        },
        extend: {
            fontFamily: {
                sans: ['Inter var', ...defaultTheme.fontFamily.sans]
            },
        },
        colors: {
            transparent: 'transparent',
            current: 'currentColor',
            black: colors.black,
            gray: colors.gray,  //for drop shadow
            white: colors.white,
            red: colors.red,
            slate: colors.slate,
        }
    },
    variants: {
        extend: {
            placeholderColor: ['focus'],
        }
    },
    daisyui: {
        themes: [
            {
                custom: {
                    "primary": "#111827",
                    "secondary": "#4b5563",
                    "accent": "#e5e7eb",
                    "neutral": "#6b7280",
                    "base-100": "#ffffff",
                    "info": "#2563eb",
                    "success": "#065f46",
                    "warning": "#fecaca",
                    "error": "#dc2626"
                }
            }
        ]
    },
    plugins: [
        require("@tailwindcss/forms"),
        require("@tailwindcss/typography"),
        require("daisyui")
    ]
}