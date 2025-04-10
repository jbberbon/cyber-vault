import * as zod from 'zod';
import {toTypedSchema} from '@vee-validate/zod';

export const registerSchema = toTypedSchema(
  zod.object({
    firstName: zod
      .string({message: 'First name is required'})
      .max(100, {message: 'First name should be less than 100 char.'}),
    lastName: zod
      .string({message: 'Last name is required'})
      .max(100, {message: 'Last name should be less than 100 char.'}),
    email: zod
      .string({message: 'Email is required.'})
      .email({message: 'Must be a valid email'}),
    password: zod
      .string({message: 'Password is required'})
      .min(8, {message: 'Password must be a minimum of 8 characters'})
      .regex(/[A-Z]/, {message: 'Password must contain at least one uppercase letter'})
      .regex(/[^a-zA-Z0-9]/, {message: 'Password must contain at least one non-alphanumeric character'})
  })
);
