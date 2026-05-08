export interface Customer {
    id?: string
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
    createdAt?: string
    updatedAt?: string
}

export interface CreateCustomerDto {
    firstName: string
    lastName: string
    email: string
    phoneNumber: string
}