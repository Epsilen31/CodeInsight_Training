// @ts-check
const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");

module.exports = tseslint.config(
  {
    files: ["**/*.ts"],
    extends: [
      eslint.configs.recommended,
      // @ts-ignore
      ...tseslint.configs.recommended,
      // @ts-ignore
      ...tseslint.configs.stylistic,
      // @ts-ignore
      ...angular.configs.tsRecommended,
      // @ts-ignore
      "plugin:prettier/recommended",
    ],
    processor: angular.processInlineTemplates,
    rules: {
      "@angular-eslint/directive-selector": [
        "error",
        {
          type: "attribute",
          prefix: "app",
          style: "camelCase",
        },
      ],
      "@angular-eslint/component-selector": [
        "error",
        {
          type: "element",
          prefix: "app",
          style: "kebab-case",
        },
      ],
      "@angular-eslint/prefer-standalone": "off", // Disabling prefer-standalone rule
      "@typescript-eslint/no-inferrable-types": "off", // Allowing type inference
      "@angular-eslint/template/click-events-have-key-events": "off", // Disabling rule for click events
      "@angular-eslint/template/interactive-supports-focus": "off", // Disabling rule for interactive elements focus support
      "@typescript-eslint/no-unused-vars": [
        "warn",
        { argsIgnorePattern: "^_" }, // Ignoring unused variables starting with _
      ],
      "@typescript-eslint/no-non-null-assertion": "off", // Disabling non-null assertion warning
      "@typescript-eslint/consistent-type-definitions": ["error", "interface"], // Enforcing consistent type definitions
      "prettier/prettier": [
        "error",
        {
          semi: false, // Disable semicolons
          trailingComma: "none", // Disable trailing commas
        },
      ],
    },
  },
  {
    files: ["**/*.html"],
    extends: [
      ...angular.configs.templateRecommended,
      ...angular.configs.templateAccessibility,
    ],
    rules: {
      "@angular-eslint/template/click-events-have-key-events": "off", // Disabling click event rule for templates
      "@angular-eslint/template/interactive-supports-focus": "off", // Disabling interactive focus rule for templates
    },
  },
);
