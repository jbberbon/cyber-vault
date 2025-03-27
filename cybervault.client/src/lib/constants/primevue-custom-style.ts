import {definePreset} from "@primeuix/themes";
import Aura from "@primeuix/themes/aura";

const customStyle = definePreset(Aura, {
  semantic: {
    primary: {
      50: '#F7F7FD',
      100: '#D7D8F6',
      200: '#B7B8EF',
      300: '#9799E7',
      400: '#777AE0',
      500: '#575bd9',
      600: '#4A4DB8',
      700: '#3D4098',
      800: '#303277',
      900: '#232457',
      950: '#161736'
    },
    surface: {
      1000: '#101010'
    }

    /*colorScheme: {
      dark: {

      }
    }*/
  }

});

export default customStyle
