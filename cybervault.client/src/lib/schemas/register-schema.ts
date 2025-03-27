import * as zod from 'zod';
import {toTypedSchema} from '@vee-validate/zod';
export const registerSchema = toTypedSchema(
  zod.object({
    firstName: zod
      .string()
      .min(1, {message: 'First name is required'})
      .max(100, {message: 'First name should be less than 100 char.'}),
    lastName: zod
      .string()
      .min(1, {message: 'Last name is required.'})
      .max(100, {message: 'Last name should be less than 100 char.'}),
    email: zod
      .string()
      .min(1, {message: 'Email is required.'})
      .email({message: 'Must be a valid email.'}),
    password: zod
      .string()
      .min(6, {message: 'Minimum of 6.'}),
  })
);
