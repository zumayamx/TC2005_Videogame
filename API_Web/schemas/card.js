import { z }from 'zod'

const cardSchema = z.object({
    name: z.string({
            invalid_type_error: 'El nombre debe ser un texto',
            required_error: 'El nombre es requerido'
    }),
    type: z.number({
        invalid_type_error: 'El tipo debe ser un número'
    }),
    energyCost: z.number({
        invalid_type_error: 'El costo de la carta debe ser un número'
    }),
    description: z.string({
        invalid_type_error: 'El nombre debe ser un texto',
        required_error: 'El nombre es requerido'
    }),
    value: z.number({
        invalid_type_error: 'La potencia de una carta debe ser un número'
    })
})

export function validateCard (object) {
    return cardSchema.safeParse(object);
}

export function validatePartialCard (object) {
    return cardSchema.partial().safeParse(object);
}