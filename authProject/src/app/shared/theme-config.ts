import { Theme, ThemeDefaultParams, themeQuartz } from 'ag-grid-community';

export const gridTheme: Theme<ThemeDefaultParams> = themeQuartz
  .withParams(
    {
      backgroundColor: '#FFE8E0',
      browserColorScheme: 'light',
      fontFamily: 'Arial, sans-serif',
      fontSize: '20px',
      borderColor: '#FFA07A',
      headerBackgroundColor: '#FFCCCC',
      foregroundColor: '#361008',
      accentColor: '#FF6347'
    },
    'light-red'
  )
  .withParams(
    {
      backgroundColor: '#1E1E2E',
      browserColorScheme: 'dark',
      fontFamily: 'Helvetica, sans-serif',
      fontSize: '20px',
      borderColor: '#8B0000',
      headerBackgroundColor: '#2B2B3A',
      foregroundColor: '#FFFFFF',
      accentColor: '#FF4500'
    },
    'dark-red'
  );
