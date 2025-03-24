import { Theme, ThemeDefaultParams, themeQuartz } from 'ag-grid-community';

export const myTheme: Theme<ThemeDefaultParams> = themeQuartz
  .withParams(
    {
      backgroundColor: '#FFE8E0',
      foregroundColor: '#361008CC',
      browserColorScheme: 'light'
    },
    'light-red'
  )
  .withParams(
    {
      backgroundColor: '#1E1E2E',
      foregroundColor: '#FFFFFFCC',
      browserColorScheme: 'dark'
    },
    'dark-red'
  );
