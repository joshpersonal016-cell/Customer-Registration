import { api } from './api'

import type { Customer, CreateCustomerDto } from '../types/customer'

export const getCustomersAsync = async (): Promise<Customer[]> => {
  const response = await api.get('/customer')

  return response.data
}

export const getCustomerByIdAsync = async (
  id: string
): Promise<Customer> => {
  const response = await api.get(`/customer/${id}`)

  return response.data
}

export const createCustomerAsync = async (
  payload: CreateCustomerDto
): Promise<string> => {
  const response = await api.post('/customer', payload)

  return response.data
}
