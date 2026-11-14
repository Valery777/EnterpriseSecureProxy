
using EnterpriseSecureProxy.src.Application.Exceptions;
using EnterpriseSecureProxy.src.Application.Interfaces;
using EnterpriseSecureProxy.src.Domain.Entities;
using EnterpriseSecureProxy.src.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EnterpriseSecureProxy.src.Application.Services
{
   /// <summary>
   /// ParameterValidator
   /// Assessment requirement:
   /// required parameters
   /// type validation
   /// unknown parameters
   /// max length
   /// ranges
   /// path traversal 
   /// </summary>
 
    public sealed class ParameterValidator : IParameterValidator
    {

       
        public ParameterValidator(IOperationRepository repository, IMemoryCache cache)
          
        {

            
        }
        public Task ValidateAsync(
            Operation operation,
            JsonElement payload,
            CancellationToken cancellationToken)
        {
           
            
            try
            {
                ValidateRequiredParameters(operation, payload);

                ValidateUnknownParameters(operation, payload);

                foreach (var rule in operation.ParameterRules)
                {
                    if (!payload.TryGetProperty(rule.Name, out var value))
                        continue;

                    ValidateDataType(rule, value);

                    ValidateLength(rule, value);

                    ValidateRange(rule, value);

                    ValidateRegex(rule, value);

                    ValidatePathTraversal(rule, value);
                }
                
               
                
                return Task.CompletedTask;
            }
            catch (Exception ex) when (
                ex is InvalidParameterException ||
                ex is PathTraversalException)
            {
             
               
                throw;
            }
            catch (Exception ex)
            {
              
               
                throw new InvalidParameterException(
                    $"Parameter validation failed: {ex.Message}");
            }
        }

        private static void ValidateRequiredParameters(
            Operation operation,
            JsonElement payload)
        {
                    
            try
            {
                
                foreach (var rule in operation.ParameterRules.Where(x => x.IsRequired))
                {
                    if (!payload.TryGetProperty(rule.Name, out _))
                    {
                        throw new InvalidParameterException(
                            $"Required parameter '{rule.Name}' is missing.");
                    }
                }
            }
            catch (Exception ex) when (
                ex is InvalidParameterException)
            {
                
                throw;
            }
            catch (Exception ex)
            {
               
                throw new InvalidParameterException(
                    $"Required parameter validation failed: {ex.Message}");
            }
        }

        private static void ValidateUnknownParameters(
            Operation operation,
            JsonElement payload)
        {
            try
            {
                if (operation.AllowUnknownParameters)
                    return;

                var allowed =
                    operation.ParameterRules
                        .Select(x => x.Name)
                        .ToHashSet(StringComparer.OrdinalIgnoreCase);

                foreach (var property in payload.EnumerateObject())
                {
                    if (!allowed.Contains(property.Name))
                    {
                        throw new InvalidParameterException(
                            $"Unknown parameter '{property.Name}'.");
                    }
                }
            }
            catch (Exception ex) when (
                ex is InvalidParameterException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException(
                    $"Unknown parameter validation failed: {ex.Message}");
            }
        }

        private static void ValidateDataType(
            ParameterRule rule,
            JsonElement value)
        {
            try { 
                switch (rule.DataType)
                {
                    case ParameterDataType.String:
                        if (value.ValueKind != JsonValueKind.String)
                            ThrowTypeError(rule);
                        break;
                    case ParameterDataType.Integer:
                        if (!value.TryGetInt32(out _))
                            ThrowTypeError(rule);
                        break;
                    case ParameterDataType.Decimal:
                        if (!value.TryGetDecimal(out _))
                            ThrowTypeError(rule);
                        break;
                    case ParameterDataType.Boolean:
                        if (value.ValueKind != JsonValueKind.True &&
                            value.ValueKind != JsonValueKind.False)
                        {
                            ThrowTypeError(rule);
                        }
                        break;
                    case ParameterDataType.Guid:
                        if (!Guid.TryParse(value.GetString(), out _))
                            ThrowTypeError(rule);
                        break;
                    case ParameterDataType.DateTime:
                        if (!DateTime.TryParse(value.GetString(), out _))
                            ThrowTypeError(rule);
                        break;
                }
            }
            catch (Exception ex) when (
                ex is InvalidParameterException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException(
                    $"Data type validation failed for parameter '{rule.Name}': {ex.Message}");
            }   
            switch (rule.DataType)
            {
                case ParameterDataType.String:
                    if (value.ValueKind != JsonValueKind.String)
                        ThrowTypeError(rule);
                    break;

                case ParameterDataType.Integer:
                    if (!value.TryGetInt32(out _))
                        ThrowTypeError(rule);
                    break;

                case ParameterDataType.Decimal:
                    if (!value.TryGetDecimal(out _))
                        ThrowTypeError(rule);
                    break;

                case ParameterDataType.Boolean:
                    if (value.ValueKind != JsonValueKind.True &&
                        value.ValueKind != JsonValueKind.False)
                    {
                        ThrowTypeError(rule);
                    }
                    break;

                case ParameterDataType.Guid:
                    if (!Guid.TryParse(value.GetString(), out _))
                        ThrowTypeError(rule);
                    break;

                case ParameterDataType.DateTime:
                    if (!DateTime.TryParse(value.GetString(), out _))
                        ThrowTypeError(rule);
                    break;
            }
        }

        private static void ValidateLength(
            ParameterRule rule,
            JsonElement value)
        {
            try
            {
                if (rule.MaxLength == null)
                    return;

                if (value.ValueKind != JsonValueKind.String)
                    return;

                var stringValue = value.GetString();

                if (stringValue?.Length > rule.MaxLength)
                {
                    throw new InvalidParameterException(
                        $"Parameter '{rule.Name}' exceeds maximum length.");
                }
            }
            catch (Exception ex) when (
                ex is InvalidParameterException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException(
                    $"Length validation failed for parameter '{rule.Name}': {ex.Message}");
            }
        }

        private static void ValidateRange(
                        ParameterRule rule,
                        JsonElement value)
        {
            switch (rule.DataType)
            {
                case ParameterDataType.Integer:
                case ParameterDataType.Decimal:
                    break;

                default:
                    return;
            }

            if (!value.TryGetDecimal(out var numericValue))
                return;

            if (rule.MinValue.HasValue &&
                numericValue < rule.MinValue.Value)
            {
                throw new InvalidParameterException(
                    $"Parameter '{rule.Name}' is below minimum allowed value.");
            }

            if (rule.MaxValue.HasValue &&
                numericValue > rule.MaxValue.Value)
            {
                throw new InvalidParameterException(
                    $"Parameter '{rule.Name}' exceeds maximum allowed value.");
            }
        }


        private static void ValidateRegex(
            ParameterRule rule,
            JsonElement value)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(rule.RegexPattern))
                    return;

                if (value.ValueKind != JsonValueKind.String)
                    return;

                var stringValue = value.GetString();

                if (!Regex.IsMatch(
                        stringValue ?? string.Empty,
                        rule.RegexPattern))
                {
                    throw new InvalidParameterException(
                        $"Parameter '{rule.Name}' failed validation.");
                }
            }
            catch (Exception ex) when (
                ex is InvalidParameterException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException(
                    $"Regex validation failed for parameter '{rule.Name}': {ex.Message}");
            }
        }

        private static void ValidatePathTraversal(
            ParameterRule rule,
            JsonElement value)
        {
            try
            {
                if (value.ValueKind != JsonValueKind.String)
                    return;

                var input = value.GetString();

                if (string.IsNullOrWhiteSpace(input))
                    return;

                var lowered = input.ToLowerInvariant();

                if (lowered.Contains("../") ||
                    lowered.Contains(@"..\") ||
                    lowered.Contains("%2e%2e") ||
                    lowered.Contains("%2f") ||
                    lowered.Contains("%5c"))
                {
                    throw new PathTraversalException(rule.Name);
                }
            }
            catch (Exception ex) when (
                ex is PathTraversalException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidParameterException(
                    $"Path traversal validation failed for parameter '{rule.Name}': {ex.Message}");
            }
        }

        private static void ThrowTypeError(ParameterRule rule)
        {
            throw new InvalidParameterException(
                $"Parameter '{rule.Name}' has invalid type.");
        }
    }
}